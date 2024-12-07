using DA.Anubis.Domain.Contract.AggregateKeys;
using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain;

/// <summary>
/// Base class for all mutations in the system.
/// </summary>
public abstract class Mutatie : Entity<MutatieId>
{
    private readonly string? _oldValue;
    /// <summary>
    /// Description of the original (old) value.
    /// </summary>
    public Option<string> OldValue => _oldValue;
    
    private readonly string? _newValue;
    /// <summary>
    /// Description of the new value.
    /// </summary>
    public Option<string> NewValue => _newValue;
    
    /// <summary>
    /// Base class for all mutations in the system.
    /// </summary>
    /// <param name="oldValue">Optional description of the old value.</param>
    /// <param name="newValue">Optional description of the new value.</param>
    protected Mutatie(Option<string> oldValue, Option<string> newValue)
    {
        _oldValue = oldValue.AsNullable();
        _newValue = newValue.AsNullable();
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Mutatie() { /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}