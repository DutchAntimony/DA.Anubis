using DA.Anubis.Domain.Contract.EntityKeys;
using DA.Options;

namespace DA.Anubis.Domain.BetaalmethodeAggregate;

/// <summary>
/// Elke mutatie op een adres wordt als adresMutatie opgeslagen.
/// Elk adres is verantwoordelijk voor diens eigen mutaties.
/// </summary>
public sealed class BetaalmethodeMutatie(Betaalmethode betaalmethode, BetaalmethodeMutatieType type, Option<string> oldValue, Option<string> newValue)
    : MutatieBase<Betaalmethode, BetaalmethodeId>(betaalmethode, oldValue, newValue)
{
    /// <summary>
    /// Het soort mutatie dat is opgetreden.
    /// </summary>
    public BetaalmethodeMutatieType MutatieType { get; } = type;
}