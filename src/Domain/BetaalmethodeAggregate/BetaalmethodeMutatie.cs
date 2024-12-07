using DA.Anubis.Domain.Contract.AggregateKeys;
using JetBrains.Annotations;

namespace DA.Anubis.Domain.BetaalmethodeAggregate;

/// <summary>
/// Elke mutatie op een adres wordt als adresMutatie opgeslagen.
/// Elk adres is verantwoordelijk voor diens eigen mutaties.
/// </summary>
public sealed class BetaalmethodeMutatie : Mutatie
{
    /// <summary>
    /// Het ID van het lid waar deze mutatie op plaatsgevonden heeft.
    /// </summary>
    public BetaalmethodeId EntityId { get; }
    
    /// <summary>
    /// Het soort mutatie dat is opgetreden.
    /// </summary>
    public BetaalmethodeMutatieType MutatieType { get; }
    
    /// <summary>
    /// Elke mutatie op een adres wordt als adresMutatie opgeslagen.
    /// Elk adres is verantwoordelijk voor diens eigen mutaties.
    /// </summary>
    public BetaalmethodeMutatie(Betaalmethode betaalmethode, BetaalmethodeMutatieType type, Option<string> oldValue, Option<string> newValue)
        : base(oldValue, newValue)
    {
        EntityId = betaalmethode.Id;
        MutatieType = type;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [UsedImplicitly]
    private BetaalmethodeMutatie(){ /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}