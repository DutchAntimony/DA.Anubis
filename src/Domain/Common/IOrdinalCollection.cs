using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.Common;

/// <summary>
/// Een collectie van OrdinalEntities.
/// Deze collectie maakt het mogelijk om de entities te verschuiven binnen de lijst en ervoor te zorgen
/// dan de ordinal posities correct blijven.
/// </summary>
public interface IOrdinalCollection<TEntity, in TEntityKey> : IEnumerable<TEntity>
    where TEntity : OrdinalEntity<TEntityKey> 
    where TEntityKey : IEntityKey, IEquatable<TEntityKey>, new()
{
    /// <summary>
    /// Gets the number of elements contained in the collection.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Adds an item to the end of the collection and sets its ordinal position based on the collection size.
    /// </summary>
    void Append(TEntity entity);

    /// <summary>
    /// Adds multiple items to the end of the collection and sets the ordinal positions based on the collection size.
    /// </summary>
    void AppendMany(params IEnumerable<TEntity> items);

    /// <summary>
    /// Move an entity, defined by its key, to the start of the collection. 
    /// </summary>
    /// <returns>Boolean indicating if the move is a success. False if the entity is not found or already at the top.</returns>
    bool MoveEntityToTheBeginning(TEntityKey entityKey);

    /// <summary>
    /// Move an entity to the start of the collection. 
    /// </summary>
    /// <returns>Boolean indicating if the move is a success. False if the entity is not found or already at the top.</returns>
    bool MoveEntityToTheBeginning(TEntity entity);

    /// <summary>
    /// Move an entity, defined by its key, one position up in the collection. 
    /// </summary>
    /// <returns>Boolean indicating if the move is a success. False if the entity is not found or already at the top.</returns>
    bool MoveEntityUp(TEntityKey entityKey);

    /// <summary>
    /// Move an entity one position up in the collection. 
    /// </summary>
    /// <returns>Boolean indicating if the move is a success. False if the entity is not found or already at the top.</returns>
    bool MoveEntityUp(TEntity entity);

    /// <summary>
    /// Move an entity, defined by its key, one position down in the collection. 
    /// </summary>
    /// <returns>Boolean indicating if the move is a success. False if the entity is not found or already at the bottom.</returns>
    bool MoveEntityDown(TEntityKey entityKey);

    /// <summary>
    /// Move an entity one position down in the collection. 
    /// </summary>
    /// <returns>Boolean indicating if the move is a success. False if the entity is not found or already at the bottom.</returns>
    bool MoveEntityDown(TEntity entity);

    /// <summary>
    /// Move an entity, defined by its key, to the bottom of the collection. 
    /// </summary>
    /// <returns>Boolean indicating if the move is a success. False if the entity is not found or already at the bottom.</returns>
    public bool MoveEntityToTheEnd(TEntityKey entityKey);

    /// <summary>
    /// Move an entity to the bottom of the collection. 
    /// </summary>
    /// <returns>Boolean indicating if the move is a success. False if the entity is not found or already at the bottom.</returns>
    public bool MoveEntityToTheEnd(TEntity entity);
}