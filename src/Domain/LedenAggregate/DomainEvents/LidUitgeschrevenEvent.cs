using DA.Anubis.Domain.Contract.AggregateKeys;
using DA.Anubis.Domain.Contract.Enumerations;
using DA.DDD.CoreLibrary.Events;

namespace DA.Anubis.Domain.LedenAggregate.DomainEvents;

/// <summary>
/// Event wat gepubliceerd wordt indien een lid wordt uitgeschreven.
/// Kan gebruikt worden om te controleren of
/// - Adres nog een geldige hoofdbewoner heeft en überhaupt nog leden heeft.
/// - Betaalwijze nog een geldig verantwoordelijk lid heeft.
/// - Contributie nota's aangepast moeten worden omdat we geen contributie meer willen innen. 
/// </summary>
public class LidUitgeschrevenEvent(Lid sender, Uitschrijfreden uitschrijfreden) : DomainEvent(sender)
{
    public LidId LidId { get; } = sender.Id;
    public Uitschrijfreden Uitschrijfreden { get; } = uitschrijfreden;
}