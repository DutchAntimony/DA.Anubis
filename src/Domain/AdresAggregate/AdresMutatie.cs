using DA.Anubis.Domain.Contract.AggregateKeys;
using JetBrains.Annotations;

namespace DA.Anubis.Domain.AdresAggregate;

/// <summary>
/// Elke mutatie op een adres wordt als adresMutatie opgeslagen.
/// Elk adres is verantwoordelijk voor diens eigen mutaties.
/// </summary>
public sealed class AdresMutatie : Mutatie
{
    /// <summary>
    /// Het ID van het adres waar deze mutatie op plaatsgevonden heeft.
    /// </summary>
    public AdresId EntityId { get; }
    
    /// <summary>
    /// Het soort mutatie dat is opgetreden.
    /// </summary>
    public AdresMutatieType MutatieType { get; }
    
    /// <summary>
    /// Elke mutatie op een adres wordt als adresMutatie opgeslagen.
    /// Elk adres is verantwoordelijk voor diens eigen mutaties.
    /// </summary>
    public AdresMutatie(Adres adres, AdresMutatieType type, Option<string> oldValue, Option<string> newValue) 
        : base(oldValue, newValue)
    {
        EntityId = adres.Id;
        MutatieType = type;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [UsedImplicitly]
    private AdresMutatie() { /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}