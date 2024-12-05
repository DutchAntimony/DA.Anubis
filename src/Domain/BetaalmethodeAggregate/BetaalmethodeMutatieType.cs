namespace DA.Anubis.Domain.BetaalmethodeAggregate;

/// <summary>
/// De verschillende soorten adres mutaties die kunnen voorkomen.
/// </summary>
public enum BetaalmethodeMutatieType
{
    NieuwVerantwoordelijkLid,
    NotaEmailVoorkeurGewijzigd,
    IncassoBankrekeningGewijzigd,
    IncassoMandaatGewijzigd
}