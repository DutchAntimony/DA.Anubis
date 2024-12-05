using DA.Results;
using DA.Results.Issues;

namespace DA.Anubis.Domain.BetaalmethodeAggregate;

/// <summary>
/// Een betaalmethode via de bank, waarbij een machtiging is afgegeven om het geld van de rekening te incasseren.
/// </summary>
public sealed class IncassoBetaalmethode : Betaalmethode
{
    /// <summary>
    /// De bankrekening waar de contributie vandaan geïncasseerd moet worden.
    /// </summary>
    public Bankrekening Bankrekening { get; private set; }
    
    /// <summary>
    /// Het mandaat dat toestemming geeft om van deze bankrekening te mogen incasseren.
    /// </summary>
    public IncassoMandaat IncassoMandaat { get; private set; }

    /// <summary>
    /// Create een nieuwe incasso betaalmethode.
    /// </summary>
    /// <param name="bankrekening">De bankrekening waar de contributie vandaan geïncasseerd moet worden</param>
    /// <param name="incassoMandaat">Het mandaat dat toestemming geeft om van deze bankrekening te mogen incasseren</param>
    /// <returns></returns>
    public static IncassoBetaalmethode Create(Bankrekening bankrekening, IncassoMandaat incassoMandaat)
    {
        return new IncassoBetaalmethode()
        {
            Bankrekening = bankrekening,
            IncassoMandaat = incassoMandaat
        };
    }
    
    /// <summary>
    /// Update het mandaat. Mandaat is een immutable value object, dus een geheel nieuw object wordt verwacht.
    /// </summary>
    /// <param name="bankrekening">De nieuwe bankrekening gegevens</param>
    public Result<IncassoBetaalmethode> UpdateBankrekening(Bankrekening bankrekening)
    {
        if (Bankrekening == bankrekening)
        {
            return new UnmodifiedWarning(typeof(Bankrekening));
        }

        var oldValue = Bankrekening;
        Bankrekening = bankrekening;
        
        MutatieCollectie.Add(new BetaalmethodeMutatie(
            betaalmethode: this,
            type: BetaalmethodeMutatieType.IncassoBankrekeningGewijzigd,
            oldValue: oldValue.ToString(),
            newValue: Bankrekening.ToString()
        ));
        
        return this;
    }
    
    /// <summary>
    /// Update het mandaat. Mandaat is een immutable value object, dus een geheel nieuw object wordt verwacht.
    /// </summary>
    /// <param name="mandaat">Het nieuwe mandaat</param>
    public Result<IncassoBetaalmethode> UpdateMandaat(IncassoMandaat mandaat)
    {
        if (IncassoMandaat == mandaat)
        {
            return new UnmodifiedWarning(typeof(IncassoMandaat));
        }

        var oldValue = IncassoMandaat;
        IncassoMandaat = mandaat;
        
        MutatieCollectie.Add(new BetaalmethodeMutatie(
            betaalmethode: this,
            type: BetaalmethodeMutatieType.IncassoMandaatGewijzigd,
            oldValue: oldValue.ToString(),
            newValue: IncassoMandaat.ToString()
            ));
        
        return this;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private IncassoBetaalmethode() { /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}