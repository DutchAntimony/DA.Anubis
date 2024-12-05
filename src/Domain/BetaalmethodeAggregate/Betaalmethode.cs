using DA.Anubis.Domain.Contract.EntityKeys;
using DA.DDD.CoreLibrary.Entities;
using DA.Guards;
using DA.Options;
using DA.Options.Extensions;
using DA.Results;
using DA.Results.Issues;

namespace DA.Anubis.Domain.BetaalmethodeAggregate;

public abstract class Betaalmethode : AggregateRoot<BetaalmethodeId>
{
    // ReSharper disable once CollectionNeverUpdated.Local - set by ORM
    private readonly ICollection<LidId> _ledenIds = [];
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
    /// Readonly collectie van alle met deze betaalwijze betalende leden.
    /// </summary>
    public IEnumerable<LidId> LedenIds => _ledenIds;

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
        if (VerantwoordelijkLidId.TryGetValue(out var oudLidId) && oudLidId == lidId)
        {
            return new UnmodifiedWarning(typeof(LidId));
        }

        if (!LedenIds.Contains(lidId))
        {
            return new InvalidOperationError(
                "Kan geen lid verantwoordelijk maken dat niet als lid op dit adres is geregistreerd.");
        }
        
        var oldValue = VerantwoordelijkLidId;
        VerantwoordelijkLidId = lidId.EnsureNotDefault();
        MutatieCollectie.Add(new BetaalmethodeMutatie(
            betaalmethode: this,
            type: BetaalmethodeMutatieType.NieuwVerantwoordelijkLid,
            oldValue: oldValue.Map(v => v.ToString()),
            newValue: VerantwoordelijkLidId.Map(v => v.ToString())
        ));
        
        return this;
    }
}