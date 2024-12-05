using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.BetaalmethodeAggregate;

/// <summary>
/// Details van een bankrekening waarvan geïncasseerd mag worden.
/// </summary>
public class Bankrekening(Iban iban, string bic, string tenNameVan) : ValueObject
{
    /// <summary>
    /// De international bank account number (IBAN) voor deze bankrekening.
    /// </summary>
    public Iban Iban { get; } = iban;

    /// <summary>
    /// De bank identification code (BIC) of swift code die hoort bij de bank van de rekening.
    /// </summary>
    public string Bic { get; } = bic;

    /// <summary>
    /// Tenaamstelling van de rekening 
    /// </summary>
    public string TenNameVan { get; } = tenNameVan;

    /// <inheritdoc />
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Iban;
    }
}