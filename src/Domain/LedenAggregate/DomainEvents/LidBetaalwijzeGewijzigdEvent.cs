using DA.Anubis.Domain.Contract.EntityKeys;
using DA.DDD.CoreLibrary.Events;
using DA.Options;

namespace DA.Anubis.Domain.LedenAggregate.DomainEvents;

/// <summary>
/// Event wat gepubliceerd wordt indien een lid een nieuwe betaalwijze krijgt
/// Kan gebruikt worden om te controleren of
/// - Betaalwijze nog een geldige verantwoordelijk lid heeft en überhaupt nog leden heeft.
/// </summary>
/// <param name="sender"></param>
/// <param name="oldBetaalmethodeId"></param>
/// <param name="newBetaalmethodeId"></param>
public class LidBetaalwijzeGewijzigdEvent(
    Lid sender,
    ValueOption<BetaalmethodeId> oldBetaalmethodeId,
    ValueOption<BetaalmethodeId> newBetaalmethodeId)
    : DomainEvent(sender)
{
    public LidId LidId { get; } = sender.Id;
    public ValueOption<BetaalmethodeId> OldBetaalmethodeId { get; } = oldBetaalmethodeId;
    public ValueOption<BetaalmethodeId> NewBetaalmethodeId { get; } = newBetaalmethodeId;
}