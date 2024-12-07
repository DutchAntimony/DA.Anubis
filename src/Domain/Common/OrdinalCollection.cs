using System.Collections;
using DA.DDD.CoreLibrary.Entities;
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
    public bool MoveEntityToTheBeginning(TEntityKey entityKey)
    {
        var entity = Collection.FirstOrDefault(entity => entity.Id.Equals(entityKey));
        return entity is not null && MoveEntityToTheBeginning(entity);
    }
    
    /// <inheritdoc />
    public bool MoveEntityToTheBeginning(TEntity entity)
    {
        if (entity.Ordinal == 1) return false; // item is already at the first index.
        Collection.Where(candidate => candidate.Ordinal < entity.Ordinal)
            .ForEach(candidate => candidate.MoveDown());
        entity.SetOrdinal(1);
        return true;
    }

    /// <inheritdoc />
    public bool MoveEntityUp(TEntityKey entityKey)
    {
        var entity = Collection.FirstOrDefault(entity => entity.Id.Equals(entityKey));
        return entity is not null && MoveEntityUp(entity);
    }
    
    /// <inheritdoc />
    public bool MoveEntityUp(TEntity entity)
    {
        if (entity.Ordinal == 1) return false; // item is already at the first index.
        var previous = Collection.FirstOrDefault(candidate => candidate.Ordinal == entity.Ordinal - 1);
        if (previous is null) return false;
        entity.MoveUp();
        previous.MoveDown();
        return true;
    }
    
    /// <inheritdoc />
    public bool MoveEntityDown(TEntityKey entityKey)
    {
        var entity = Collection.FirstOrDefault(entity => entity.Id.Equals(entityKey));
        return entity is not null && MoveEntityDown(entity);
    }
    
    /// <inheritdoc />
    public bool MoveEntityDown(TEntity entity)
    {
        if (entity.Ordinal == Count) return false; // item is already at the last index.
        var next = Collection.FirstOrDefault(candidate => candidate.Ordinal == entity.Ordinal + 1);
        if (next is null) return false;
        entity.MoveDown();
        next.MoveUp();
        return true;
    }
    
    /// <inheritdoc />
    public bool MoveEntityToTheEnd(TEntityKey entityKey)
    {
        var entity = Collection.FirstOrDefault(entity => entity.Id.Equals(entityKey));
        return entity is not null && MoveEntityToTheEnd(entity);
    }
    
    /// <inheritdoc />
    public bool MoveEntityToTheEnd(TEntity entity)
    {
        if (entity.Ordinal == Count) return false; // item is already at the last index.
        Collection.Where(candidate => candidate.Ordinal > entity.Ordinal)
            .ForEach(candidate => candidate.MoveUp());
        entity.SetOrdinal(Count);
        return true;
    }
    
    public IEnumerator<TEntity> GetEnumerator() => Collection.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}