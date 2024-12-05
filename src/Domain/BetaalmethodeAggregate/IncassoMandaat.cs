using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.BetaalmethodeAggregate;

/// <summary>
/// Details over het afgegeven incasso mandaat.
/// </summary>
public class IncassoMandaat(MandaatType type, string rekeningnummer, DateOnly tekendatum, string tekenInformatie) : ValueObject
{
    /// <summary>
    /// Het type mandaat dat is afgegeven.
    /// </summary>
    public MandaatType Type { get; } = type;
    
    /// <summary>
    /// De datum waarop het mandaat is ondertekend.
    /// </summary>
    public DateOnly Tekendatum { get; } = tekendatum;
    
    /// <summary>
    /// Informatie over diegene die dit mandaat heeft getekend, een naam of een email adres.
    /// </summary>
    public string TekenInformatie { get; } = tekenInformatie;

    /// <summary>
    /// Het rekeningnummer waarvoor origineel mandaat is afgegeven.
    /// </summary>
    public string Rekeningnummer { get; } = rekeningnummer;

    public override string ToString() => $"{Type}: {Rekeningnummer} ({Tekendatum:dd-MM-yyyy})";

    /// <inheritdoc />
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Tekendatum;
        yield return Rekeningnummer;
    }
}