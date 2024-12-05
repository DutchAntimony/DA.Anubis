using DA.Anubis.Domain.Contract.EntityKeys;
using DA.DDD.CoreLibrary.Events;
using DA.Options;

namespace DA.Anubis.Domain.LedenAggregate.DomainEvents;

/// <summary>
/// Event wat gepubliceerd wordt indien een lid verhuist of geen bekend adres meer heeft
/// Kan gebruikt worden om te controleren of
/// - Adres nog een geldige hoofdbewoner heeft en überhaupt nog leden heeft.
/// </summary>
/// <param name="sender"></param>
/// <param name="oldAdresId"></param>
/// <param name="newAdresId"></param>
public class LidAdresGewijzigdEvent(Lid sender, ValueOption<AdresId> oldAdresId, ValueOption<AdresId> newAdresId)
    : DomainEvent(sender)
{
    public LidId LidId { get; } = sender.Id;
    public ValueOption<AdresId> OldAdresId {get;} = oldAdresId;
    public ValueOption<AdresId> NewAdresId {get;} = newAdresId;
}