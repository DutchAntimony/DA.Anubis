namespace DA.Anubis.Domain.BetaalmethodeAggregate;

/// <summary>
/// Een betaalmethode per post of eventueel per e-mail indien daar de voorkeur bij ligt.
/// </summary>
public sealed class NotaBetaalmethode : Betaalmethode
{
    /// <summary>
    /// Ontvangt het verantwoordelijke lid de nota bij voorkeur via e-mail?
    /// </summary>
    public bool PrefereerEmail { get; private set; }

    /// <summary>
    /// Create een nieuwe nota betaalmethode.
    /// </summary>
    /// <param name="prefereerEmail">Ontvangt het verantwoordelijke lid de nota bij voorkeur via e-mail?</param>
    /// <returns></returns>
    public static NotaBetaalmethode Create(bool prefereerEmail = false)
    {
        return new NotaBetaalmethode()
        {
            PrefereerEmail = prefereerEmail
        };
    }

    /// <summary>
    /// Update de e-mail voorkeur voor deze nota.
    /// </summary>
    public Result<NotaBetaalmethode> UpdateEmailVoorkeur(bool prefereerEmail)
    {
        if (PrefereerEmail == prefereerEmail)
        {
            return new UnmodifiedWarning(typeof(bool));
        }
        
        PrefereerEmail = prefereerEmail;
        MutatieCollectie.Add(new BetaalmethodeMutatie(
            betaalmethode: this,
            type: BetaalmethodeMutatieType.NotaEmailVoorkeurGewijzigd,
            oldValue: (!prefereerEmail).ToString(),
            newValue: prefereerEmail.ToString()
        ));

        return this;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private NotaBetaalmethode() { /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}