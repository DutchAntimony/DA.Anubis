using DA.Anubis.Domain.Contract.Enumerations;
using DA.DDD.CoreLibrary.Entities;
using DA.Guards;
using DA.Options;
using DA.Options.Extensions;

namespace DA.Anubis.Domain.LedenAggregate;

/// <summary>
/// Persoonsgegevens van een lid.
/// Dit is een immutable class,
/// indien persoonsgegevens aangepast moeten worden wordt een nieuwe instance van deze class aangemaakt.
/// </summary>
public sealed class Personalia : ValueObject
{
    private readonly string? _tussenvoegsel;
    
    /// <summary>
    /// Voorletter(s) van het lid, gescheiden door een '.'<br/>
    /// Ligaturen zoals Ch, Sch, Sj, Tj en Tsj zijn toegestaan.
    /// </summary>
    public string Voorletters { get; }
    
    /// <summary>
    /// Optioneel, tussenvoegsel bij de eerste achternaam, om sortering te verbeteren.
    /// </summary>
    public Option<string> Tussenvoegsel
    {
        get => _tussenvoegsel;
        init => _tussenvoegsel = value.AsNullable();
    }
    
    /// <summary>
    /// Achternaam van het lid. Eventuele dubbele achternamen worden met toevoegsel achter de eerste achternaam gezet.
    /// </summary>
    public string Achternaam { get; }
    
    /// <summary>
    /// Geslacht zoals opgegeven. Wordt gebruikt voor de aanspreeknaam.
    /// Veld is verplicht, gebruik <see cref="Geslacht.Onbekend"/> indien het geslacht niet opgegeven is.
    /// </summary>
    public Geslacht Geslacht { get;  }
    
    /// <summary>
    /// Geboortedatum van het lid. Wordt gebruikt voor de leeftijd bepaling.
    /// </summary>
    public DateOnly Geboortedatum { get; }

    /// <summary>
    /// Maak een nieuw Personalia object aan.
    /// </summary>
    /// <param name="voorletters">Voorletter(s) van het lid. Ligaturen zoals Ch, Sj en Tj zijn toegestaan.</param>
    /// <param name="tussenvoegsel">Optioneel, tussenvoegsel bij de eerste achternaam, om sortering te verbeteren.</param>
    /// <param name="achternaam">Achternaam van het lid. Eventuele dubbele achternamen worden met toevoegsel achter de eerste achternaam gezet.</param>
    /// <param name="geslacht">Geslacht zoals opgegeven, gebruik <see cref="Geslacht.Onbekend"/> indien het geslacht niet opgegeven is.</param>
    /// <param name="geboortedatum">Geboortedatum van het lid.</param>
    public Personalia(string voorletters, Option<string> tussenvoegsel, string achternaam, Geslacht geslacht, DateOnly geboortedatum)
    {
        Voorletters = voorletters.EnsureNotEmpty();
        Tussenvoegsel = tussenvoegsel;
        Achternaam = achternaam.EnsureNotEmpty();
        Geslacht = geslacht;
        Geboortedatum = geboortedatum.EnsureNotDefault();
    }

    private string GetVolledigeNaam()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Geef de volledige naam van het lid weer, inclusief titel
    /// </summary>
    /// <example>Mevr. Sj.A. van Buuren-de Boer</example>
    public string GetVolledigeNaamEnTitel()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Geef de volledige naam van het lid weer, inclusief geboortedatum
    /// </summary>
    /// <example>Mevr. Sj.A. van Buuren-de Boer (12-05-1986)</example>
    public string GetVolledigeNaamMetGeboortedatum() =>
        $"{GetVolledigeNaamEnTitel()} ({Geboortedatum:dd-MM-yyyy})";
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Personalia() { /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <inheritdoc cref="ValueObject.GetAtomicValues"/>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Voorletters;
        yield return Achternaam;
        yield return Geslacht;
        yield return Geboortedatum;
        if (_tussenvoegsel is not null)
        {
            yield return _tussenvoegsel;
        }
    }
}