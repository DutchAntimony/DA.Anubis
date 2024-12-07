using DA.Anubis.Domain.Contract.AggregateKeys;
using DA.Anubis.Domain.Contract.Enumerations;
using DA.Anubis.Domain.LedenAggregate.DomainEvents;
using DA.DDD.CoreLibrary.Entities;
using DA.DDD.CoreLibrary.Entities.Auditing;

namespace DA.Anubis.Domain.LedenAggregate;

/// <summary>
/// Gegevens van een lid.
/// </summary>
public sealed class Lid : AggregateRoot<LidId>, ISoftDeletableEntity
{
    private readonly List<Emailadres> _emailadressen = [];
    private readonly List<Telefoonnummer> _telefoonnummers = [];
    private readonly List<Notitie> _notities = [];
    private readonly List<LidMutatie> _mutaties = [];

    private AdresId? _adresId;
    private BetaalmethodeId? _betaalmethodeId;
    private Uitschrijfreden? _uitschrijfreden;
    
    /// <summary>
    /// Het unieke lidnummer voor dit lid.
    /// </summary>
    public int Lidnummer { get; private init;}
    
    /// <summary>
    /// Persoonsgegevens van dit lid.
    /// </summary>
    public Personalia Personalia { get; private set; }

    /// <summary>
    /// De identifier van het adres waar dit lid woont.
    /// Is optioneel, het kan zijn dat van een lid geen adres bekend is.
    /// </summary>
    public ValueOption<AdresId> AdresId
    {
        get => _adresId;
        private set => _adresId = value.AsNullable();
    }
    
    /// <summary>
    /// De identifier van de methode waarmee dit lid contributie betaald.
    /// Is optioneel, kinderen onder de 18 betalen geen contributie.
    /// </summary>
    public ValueOption<BetaalmethodeId> BetaalmethodeId
    {
        get => _betaalmethodeId;
        private set => _betaalmethodeId = value.AsNullable();
    }
    
    /// <summary>
    /// De reden van uitschrijving indien dit lid is uitgeschreven.
    /// Is optioneel, de meeste leden zullen niet uitgeschreven zijn.
    /// </summary>
    public ValueOption<Uitschrijfreden> Uitschrijfreden 
    {
        get => _uitschrijfreden;
        private set => _uitschrijfreden = value.AsNullable();
    }
    
    /// <summary>
    /// Collectie van alle email adressen van dit lid.
    /// </summary>
    public IEnumerable<Emailadres> Emailadressen => _emailadressen;

    /// <summary>
    /// Collectie van alle telefoonnummers van dit lid.
    /// </summary>
    public IEnumerable<Telefoonnummer> Telefoonnummers => _telefoonnummers;

    /// <summary>
    /// Collectie van alle notities van dit lid.
    /// </summary>
    public IEnumerable<Notitie> Notities => _notities;

    /// <summary>
    /// Collectie van alle mutaties die hebben plaatsgevonden op dit adres.
    /// </summary>
    public IEnumerable<LidMutatie> Mutaties => _mutaties;
    
    /// <summary>
    /// Een lid wordt niet uit de database verwijderd. In plaats daarvan wordt deletion info gebruikt
    /// waarin staat wie en wanneer dit lid verwijderd (uitgeschreven) is.
    /// </summary>
    public SoftDeletionInfo DeletionInfo { get; } = new();

    /// <summary>
    /// Create een nieuw lid.
    /// Een adres is verplicht voor een nieuw lid; alleen door een onduidelijke verhuizing in de toekomst kan dit null worden.
    /// </summary>
    /// <param name="lidnummer">Het unieke lidnummer voor dit lid.</param>
    /// <param name="personalia">Persoonsgegevens van dit lid.</param>
    /// <param name="adresId">De identifier van het adres waar dit lid woont.</param>
    /// <param name="betaalmethodeId">De identifier van de methode waarmee dit lid contributie betaald. Is optioneel, kinderen onder de 18 betalen geen contributie.</param>
    public static Lid Create(int lidnummer, Personalia personalia, AdresId adresId, ValueOption<BetaalmethodeId> betaalmethodeId)
    {
        return new Lid()
        {
            Lidnummer = lidnummer.EnsurePositive(),
            Personalia = personalia,
            AdresId = adresId.EnsureNotDefault(),
            BetaalmethodeId = betaalmethodeId
        };
    }

    /// <summary>
    /// Geef de volledige naam van het lid weer, inclusief geboortedatum en lidnummer
    /// </summary>
    /// <example>12 - Sj.A. van Buuren-de Boer (12-05-1986)</example>
    public string GetVolledigeNaamMetLidnummer() => 
        $"{Lidnummer} - {Personalia.GetVolledigeNaamMetGeboortedatum()}";

    /// <summary>
    /// Geef nieuwe, verbeterde Personalia door voor dit lid.
    /// </summary>
    /// <param name="personalia">De verbeterde waarden.</param>
    /// <returns></returns>
    public Result<Lid> CorrigeerPersonalia(Personalia personalia)
    {
        if (Personalia.Equals(personalia))
        {
            return new UnmodifiedWarning(typeof(Personalia));
        }
        
        var origineel = Personalia.GetVolledigeNaamMetGeboortedatum();
        Personalia = personalia;
        
        _mutaties.Add(new LidMutatie(
            lid: this, 
            type: LidMutatieType.PersonaliaCorrectie,
            oldValue: origineel,
            newValue: Personalia.GetVolledigeNaamMetGeboortedatum()
        ));

        return this;
    }

    /// <summary>
    /// Schrijf dit lid uit als lid van de vereniging.
    /// </summary>
    /// <param name="uitschrijfreden">De reden waarom dit lid wordt uitgeschreven.</param>
    /// <remarks>Publishes a <see cref="LidUitgeschrevenEvent"/></remarks>
    public Result<Lid> SchrijfUit(Uitschrijfreden uitschrijfreden)
    {
        if (Uitschrijfreden.HasValue)
        {
            return new InvalidOperationError("Kan reeds uitgeschreven lid niet nogmaals uitschrijven.");
        }

        Uitschrijfreden = uitschrijfreden;
        
        _mutaties.Add(new LidMutatie(
            lid: this, 
            type: LidMutatieType.Uitschrijving,
            oldValue: Option.None,
            newValue: uitschrijfreden.ToString()
        ));

        AddDomainEvent(new LidUitgeschrevenEvent(this, uitschrijfreden));
        
        return this;
    }

    /// <summary>
    /// Verhuis het lid naar een nieuw, bestaand adres.
    /// Indien het lid verhuist naar een nieuw adres dient eerst het adres aangemaakt te worden.
    /// </summary>
    /// <param name="adresId">De identifier van het adres waar dit lid heen verhuist.</param>
    public Result<Lid> Verhuis(AdresId adresId)
    {
        if (AdresId.TryGetValue(out var oudAdresId) && oudAdresId == adresId)
        {
            return new UnmodifiedWarning(typeof(AdresId));
        }

        var oldAdresId = AdresId;
        AdresId = adresId.EnsureNotDefault();
        
        _mutaties.Add(new LidMutatie(
            lid: this, 
            type: LidMutatieType.Adreswijziging,
            oldValue: oldAdresId.Map(v => v.ToString()),
            newValue: AdresId.Map(v => v.ToString())
        ));
        
        return this;
    }

    /// <summary>
    /// Set de betaalmethode van het lid.
    /// Dit kan een eerste betaalmethode zijn (bij een nieuw betalend lid) of een update van de methode.
    /// </summary>
    /// <param name="betaalmethodeId">De identifier van de nieuwe betaalmethode.</param>
    public Result<Lid> SetOrUpdateBetaalmethode(BetaalmethodeId betaalmethodeId)
    {
        if (BetaalmethodeId.TryGetValue(out var oudBetaalmethodeId) && oudBetaalmethodeId == betaalmethodeId)
        {
            return new UnmodifiedWarning(typeof(BetaalmethodeId));
        }
        
        var oldBetaalmethodeId = BetaalmethodeId;
        BetaalmethodeId = betaalmethodeId.EnsureNotDefault();
        
        _mutaties.Add(new LidMutatie(
            lid: this, 
            type: LidMutatieType.BetaalmethodeWijziging,
            oldValue: oldBetaalmethodeId.Map(v => v.ToString()),
            newValue: BetaalmethodeId.Map(v => v.ToString())
        ));
        
        AddDomainEvent(new LidBetaalwijzeGewijzigdEvent(this, oldBetaalmethodeId, BetaalmethodeId));
        return this;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Lid() { /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}