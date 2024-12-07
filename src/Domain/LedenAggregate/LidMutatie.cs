using DA.Anubis.Domain.Contract.AggregateKeys;
using JetBrains.Annotations;

namespace DA.Anubis.Domain.LedenAggregate;

/// <summary>
/// Elke mutatie op een lid wordt als lidMutatie opgeslagen.
/// Elk lid is verantwoordelijk voor diens eigen mutaties.
/// </summary>
public sealed class LidMutatie : Mutatie
{
    /// <summary>
    /// Het ID van het lid waar deze mutatie op plaatsgevonden heeft.
    /// </summary>
    public LidId EntityId { get; }
    
    /// <summary>
    /// Het soort mutatie dat is opgetreden.
    /// </summary>
    public LidMutatieType MutatieType { get; }

    /// <summary>
    /// Elke mutatie op een lid wordt als lidMutatie opgeslagen.
    /// Elk lid is verantwoordelijk voor diens eigen mutaties.
    /// </summary>
    public LidMutatie(Lid lid, LidMutatieType type, Option<string> oldValue, Option<string> newValue)
        : base(oldValue, newValue)
    {
        EntityId = lid.Id;
        MutatieType = type;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [UsedImplicitly]
    private LidMutatie() { /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}