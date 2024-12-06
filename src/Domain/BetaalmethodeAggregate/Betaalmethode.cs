using DA.Anubis.Domain.Contract.AggregateKeys;
using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.BetaalmethodeAggregate;

public abstract class Betaalmethode : AggregateRoot<BetaalmethodeId>
{
    private LidId? _verantwoordelijkLidId;
    protected readonly ICollection<BetaalmethodeMutatie> MutatieCollectie = [];

    /// <summary>
    /// De identifier van het lid dat verantwoordelijk is voor deze betaalmethode.
    /// Dit lid ontvangt de nota's en is aanspreekpersoon indien een incasso niet uitgevoerd kan worden.
    /// </summary>
    public ValueOption<LidId> VerantwoordelijkLidId
    {
        get => _verantwoordelijkLidId;
        private set => _verantwoordelijkLidId = value.AsNullable();
    }

    /// <summary>
    /// Collectie van alle mutaties die hebben plaatsgevonden op deze betaalwijze.
    /// </summary>
    public IEnumerable<BetaalmethodeMutatie> Mutaties => MutatieCollectie;
    
    /// <summary>
    /// Set een verantwoordelijk lid Id of update een bestaand verantwoordelijk lid id.
    /// </summary>
    /// <param name="lidId">Het nieuwe lidId dat verantwoordelijk is voor deze betaalmethode.</param>
    public Result<Betaalmethode> SetOrUpdateVerantwoordelijkLidId(LidId lidId)
    {
        if (VerantwoordelijkLidId.TryGetValue(out var oudLidId))
        {
            if (oudLidId == lidId)
            {
                return new UnmodifiedWarning(typeof(LidId));
            }

            // we boeken alleen een mutatie op bij een wijziging, niet bij de eerste set actie.
            MutatieCollectie.Add(new BetaalmethodeMutatie(
                betaalmethode: this,
                type: BetaalmethodeMutatieType.NieuwVerantwoordelijkLid,
                oldValue: oudLidId.Value.ToString(),
                newValue: lidId.Value.ToString()
            ));
        }

        VerantwoordelijkLidId = lidId.EnsureNotDefault();
        return this;
    }
}