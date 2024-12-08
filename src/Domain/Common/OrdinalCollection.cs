using System.Collections;
using DA.DDD.CoreLibrary.Entities;
using DA.DDD.CoreLibrary.Errors;
using DA.DDD.CoreLibrary.Extensions;

namespace DA.Anubis.Domain.Common;

/// <inheritdoc cref="IOrdinalCollection{TEntity,TEntityKey}"/>
/// <param name="collection">De basis collectie</param>
public class OrdinalCollection<TEntity, TEntityKey>(ICollection<TEntity> collection) : IOrdinalCollection<TEntity, TEntityKey>
    where TEntity : OrdinalEntity<TEntityKey> 
    where TEntityKey : IEntityKey, IEquatable<TEntityKey>, new()
{
    private ICollection<TEntity> Collection { get; } = collection;
    
    /// <inheritdoc />
    public int Count => Collection.Count;
    
    /// <inheritdoc />
    public void Append(TEntity item)
    {
        item.SetOrdinal(Count + 1);
        Collection.Add(item);
    }

    /// <inheritdoc />
    public void AppendMany(IEnumerable<TEntity> items) => items.ForEach(Append);

    /// <inheritdoc />
    public NoContentResult Remove(TEntityKey entityKey)
    {
        return Collection.SingleOrEntityNotFound(entityKey)
            .Tap(Remove)
            .WithoutContent();
    }

    /// <inheritdoc />
    public void Remove(TEntity entity)
    {
        Collection.Where(candidate => candidate.Ordinal > entity.Ordinal)
            .ForEach(candidate => candidate.MoveUp());
        Collection.Remove(entity);
    }

    /// <inheritdoc />
    public Result<TEntity> GetEntityAtOrdinal(int ordinal)
    {
        var entity = Collection.FirstOrDefault(candidate => candidate.Ordinal == ordinal);
        return entity is null
            ? NoResultsError.Create<TEntity>($"By ordinal {ordinal}")
            : entity;
    }
    
    /// <inheritdoc />
    public NoContentResult MoveEntityToTheBeginning(TEntityKey entityKey)
    {
        return Collection.SingleOrEntityNotFound(entityKey)
            .Check(MoveEntityToTheBeginning)
            .WithoutContent();
    }
    
    /// <inheritdoc />
    public NoContentResult MoveEntityToTheBeginning(TEntity entity)
    {
        return Result.Ok(entity)
            .Check(e => ValidIfOrdinalIsNot(e, 1))
            .Tap(_ =>
            {
                Collection.Where(candidate => candidate.Ordinal < entity.Ordinal)
                    .ForEach(candidate => candidate.MoveDown());
                entity.SetOrdinal(1);
            })
            .WithoutContent();
    }

    /// <inheritdoc />
    public NoContentResult MoveEntityUp(TEntityKey entityKey)
    {
        return Collection.SingleOrEntityNotFound(entityKey)
            .Check(MoveEntityUp)
            .WithoutContent();
    }
    
    /// <inheritdoc />
    public NoContentResult MoveEntityUp(TEntity entity)
    {
        return Result.Ok(entity)
            .Check(e => ValidIfOrdinalIsNot(e, 1))
            .Combine(e => GetEntityAtOrdinal(e.Ordinal - 1))
            .Tap((current, previous) =>
            {
                current.MoveUp();
                previous.MoveDown();
            })
            .WithoutContent();
    }
    
    /// <inheritdoc />
    public NoContentResult MoveEntityDown(TEntityKey entityKey)
    {
        return Collection.SingleOrEntityNotFound(entityKey)
            .Check(MoveEntityDown)
            .WithoutContent();
    }
    
    /// <inheritdoc />
    public NoContentResult MoveEntityDown(TEntity entity)
    {
        return Result.Ok(entity)
            .Check(e => ValidIfOrdinalIsNot(e, Count))
            .Combine(e => GetEntityAtOrdinal(e.Ordinal + 1))
            .Tap((current, next) =>
            {
                current.MoveDown();
                next.MoveUp();
            })
            .WithoutContent();
    }
    
    /// <inheritdoc />
    public NoContentResult MoveEntityToTheEnd(TEntityKey entityKey)
    {
        return Collection.SingleOrEntityNotFound(entityKey)
            .Check(MoveEntityToTheEnd)
            .WithoutContent();
    }
    
    /// <inheritdoc />
    public NoContentResult MoveEntityToTheEnd(TEntity entity)
    {
        return Result.Ok(entity)
            .Check(e => ValidIfOrdinalIsNot(e, Count))
            .Tap(_ =>
            {
                Collection.Where(candidate => candidate.Ordinal > entity.Ordinal)
                    .ForEach(candidate => candidate.MoveUp());
                entity.SetOrdinal(Count);
            })
            .WithoutContent();
    }
    
    public IEnumerator<TEntity> GetEnumerator() => Collection.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    
    private static NoContentResult ValidIfOrdinalIsNot(TEntity entity, int ordinal)
    {
        return entity.Ordinal == ordinal
            ? new UnmodifiedWarning(typeof(TEntity))
            : Result.Ok();
    }
}