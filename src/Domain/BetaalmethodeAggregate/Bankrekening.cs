using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.BetaalmethodeAggregate;

/// <summary>
/// Details van een bankrekening waarvan geïncasseerd mag worden.
/// </summary>
public class Bankrekening : ValueObject
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="iban">De international bank account number (IBAN) voor deze bankrekening.</param>
    /// <param name="bic">De bank identification code (BIC) of swift code die hoort bij de bank van de rekening.</param>
    /// <param name="tenNameVan">Tenaamstelling van de rekening.</param>
    public Bankrekening(Iban iban, string bic, string tenNameVan) =>
        (Iban, Bic, TenNameVan) = (iban, bic, tenNameVan);
    
    /// <summary>
    /// De international bank account number (IBAN) voor deze bankrekening.
    /// </summary>
    public Iban Iban { get; }

    /// <summary>
    /// De bank identification code (BIC) of swift code die hoort bij de bank van de rekening.
    /// </summary>
    public string Bic { get; }

    /// <summary>
    /// Tenaamstelling van de rekening 
    /// </summary>
    public string TenNameVan { get; }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Iban;
        yield return TenNameVan;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Bankrekening() { /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}