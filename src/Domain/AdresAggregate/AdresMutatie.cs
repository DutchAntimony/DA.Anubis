using DA.Anubis.Domain.Contract.EntityKeys;
using DA.Options;

namespace DA.Anubis.Domain.AdresAggregate;

/// <summary>
/// Elke mutatie op een adres wordt als adresMutatie opgeslagen.
/// Elk adres is verantwoordelijk voor diens eigen mutaties.
/// </summary>
public sealed class AdresMutatie(Adres adres, AdresMutatieType type, Option<string> oldValue, Option<string> newValue)
    : MutatieBase<Adres, AdresId>(adres, oldValue, newValue)
{
    /// <summary>
    /// Het soort mutatie dat is opgetreden.
    /// </summary>
    public AdresMutatieType MutatieType { get; } = type;
}