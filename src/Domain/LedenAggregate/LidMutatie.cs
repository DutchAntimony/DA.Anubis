using DA.Anubis.Domain.Contract.AggregateKeys;

namespace DA.Anubis.Domain.LedenAggregate;

/// <summary>
/// Elke mutatie op een lid wordt als lidMutatie opgeslagen.
/// Elk lid is verantwoordelijk voor diens eigen mutaties.
/// </summary>
public sealed class LidMutatie(Lid lid, LidMutatieType type, Option<string> oldValue, Option<string> newValue)
    : MutatieBase<Lid, LidId>(lid, oldValue, newValue)
{
    /// <summary>
    /// Het soort mutatie dat is opgetreden.
    /// </summary>
    public LidMutatieType MutatieType { get; } = type;
}