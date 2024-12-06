using DA.Anubis.Domain.Contract.AggregateKeys;
using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain;

/// <summary>
/// Base class for all mutations in the system.
/// </summary>
/// <param name="entity">The entity (aggregate root) that caused the mutation.</param>
/// <param name="oldValue">Optional description of the old value.</param>
/// <param name="newValue">Optional description of the new value.</param>
public abstract class MutatieBase<TAggregate, TKey>
    (TAggregate entity, Option<string> oldValue, Option<string> newValue) : Entity<MutatieId>
    where TAggregate : AggregateRoot<TKey> 
    where TKey : IEntityKey, IEquatable<TKey>, new()
{
    /// <summary>
    /// The key of the entity that caused the mutation.
    /// </summary>
    public TKey EntityId { get; } = entity.Id;
    
    private readonly string? _oldValue = oldValue.AsNullable();
    /// <summary>
    /// Description of the original (old) value.
    /// </summary>
    public Option<string> OldValue => _oldValue;
    
    private readonly string? _newValue = newValue.AsNullable();
    /// <summary>
    /// Description of the new value.
    /// </summary>
    public Option<string> NewValue => _newValue;
}