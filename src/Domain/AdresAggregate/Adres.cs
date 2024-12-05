using DA.Anubis.Domain.Contract.EntityKeys;
using DA.DDD.CoreLibrary.Entities;
using DA.DDD.CoreLibrary.Entities.Auditing;
using DA.Guards;
using DA.Options;
using DA.Options.Extensions;
using DA.Results;
using DA.Results.Issues;

namespace DA.Anubis.Domain.AdresAggregate;

/// <summary>
/// Het fysieke adres waar 1 of meer leden wonen.
/// </summary>
public sealed class Adres : AggregateRoot<AdresId>, ISoftDeletableEntity
{
    private LidId? _hoofdbewonerId;
    private readonly ICollection<LidId> _ledenIds = [];
    private readonly List<AdresMutatie> _mutaties = [];

    /// <summary>
    /// De identifier van het lid dat verantwoordelijk is voor dit adres.
    /// Dit lid ontvangt de post op dit adres en is verantwoordelijk voor de betaling van nota's naar dit adres.
    /// </summary>
    public ValueOption<LidId> HoofdbewonerId
    {
        get => _hoofdbewonerId;
        private set => _hoofdbewonerId = value.AsNullable();
    }
    
    /// <summary>
    /// Straat zonder huisnummer
    /// </summary>
    public string Straatnaam { get; private set; }
    
    /// <summary>
    /// Huisnummer inclusief toevoegsel
    /// </summary>
    public string Huisnummer { get; private set; }
    
    /// <summary>
    /// Postcode, kan zowel binnen NL als buitenland zijn
    /// </summary>
    public string Postcode { get; private set; }
    
    /// <summary>
    /// Woonplaats, wordt vaak afgebeeld in hoofdletters, kan zonder hoofdletters opgeslagen worden.
    /// </summary>
    public string Woonplaats { get; private set; }

    private string? _land;
    /// <summary>
    /// Land, is optioneel, indien niet ingevuld wordt uitgegaan van Nederland.
    /// </summary>
    public Option<string> Land => _land;
    
    /// <summary>
    /// Collectie van alle leden woonachtig op dit adres.
    /// </summary>
    public IEnumerable<LidId> LedenIds => _ledenIds;
    
    /// <summary>
    /// Collectie van alle mutaties die hebben plaatsgevonden op dit adres.
    /// </summary>
    public IEnumerable<AdresMutatie> Mutaties => _mutaties;

    /// <summary>
    /// Een adres wordt niet uit de database verwijderd. In plaats daarvan wordt deletion info gebruikt
    /// waarin staat wie en wanneer dit adres verwijderd (dat wil zeggen, geen koppeling meer met leden heeft) is.
    /// </summary>
    public SoftDeletionInfo DeletionInfo { get; } = new();
    
    /// <summary>
    /// Create een nieuw adres.
    /// Een hoofdbewoner is in deze fase van het proces nog niet bekend, omdat eerst het adres wordt opgeboekt en daarna pas de leden op het adres.
    /// Gebruik de methode <see cref="SetOrUpdateHoofdbewoner"/> om het hoofdbewonerId toe te voegen.
    /// </summary>
    /// <param name="straatnaam">Straat zonder huisnummer</param>
    /// <param name="huisnummer">Huisnummer inclusief toevoegsel</param>
    /// <param name="postcode">Postcode, kan zowel binnen NL als buitenland zijn</param>
    /// <param name="woonplaats">Woonplaats, wordt vaak afgebeeld in hoofdletters, kan zonder hoofdletters opgeslagen worden</param>
    /// <param name="land">Land, is optioneel, indien niet ingevuld wordt uitgegaan van Nederland</param>
    public static Adres Create(string straatnaam, string huisnummer, string postcode, string woonplaats, Option<string> land)
    {
        return new Adres
        {
            Straatnaam = straatnaam.EnsureNotEmpty(),
            Huisnummer = huisnummer.EnsureNotEmpty(),
            Postcode = postcode.EnsureNotEmpty(),
            Woonplaats = woonplaats.EnsureNotEmpty(),
            _land = land.AsNullable()
        };
    }

    /// <summary>
    /// Display het adres op twee regels.
    /// De land-afkorting wordt alleen getoond indien buiten Nederland.
    /// </summary>
    /// <example>
    /// Straatnaam 123A<br/>
    /// 1234 Brussel (B)
    /// </example>
    public string GetAdresOnTwoLines() => $"{Straatnaam} {Huisnummer}{Environment.NewLine}" +
                                          $"{Postcode.ToUpper()}  {Woonplaats.ToUpper()}{Land.Map(land => $" ({land})").Reduce("")}";

    
    /// <summary>
    /// Update het verantwoordelijke lid en boek een mutatie op.
    /// </summary>
    public void SetOrUpdateHoofdbewoner(LidId lidId)
    {
        // we boeken alleen een mutatie op bij een wijziging, niet bij de eerste set actie.
        if (HoofdbewonerId.TryGetValue(out var id))
        {
            _mutaties.Add(new AdresMutatie(
                adres: this, 
                type: AdresMutatieType.NieuwVerantwoordelijkLid,
                oldValue: id.ToString(),
                newValue: lidId.Value.ToString()
            ));
        }

        HoofdbewonerId = lidId.EnsureNotDefault();
    }
    
    /// <summary>
    /// Corrigeer een invoerfout in het adres en boek een mutatie op. <br/>
    /// Gebruik deze methode niet om een adreswijziging door te voeren.
    /// Een adreswijziging moet gedaan worden op het lid aggregate.
    /// </summary>
    /// <param name="straatnaam">De correcte straatnaam</param>
    /// <param name="huisnummer">Het correcte huisnummer</param>
    /// <param name="postcode">Het correcte postcode</param>
    /// <param name="woonplaats">De correcte woonplaats</param>
    /// <param name="land">Het correcte land</param>
    public Result<Adres> CorrigeerAdres(string straatnaam, string huisnummer, string postcode, string woonplaats, Option<string> land)
    {
        
        // todo: overweeg nog een refactor slag zodat alleen het gewijzigde veld wordt opgeslagen in de mutatie log. 
        
        return NumberOfChangedProperties() switch
        {
            0 => new UnmodifiedWarning(typeof(Adres)),
            1 => UpdateFields(straatnaam, huisnummer, postcode, woonplaats, land),
            _ => CreateValidationError("Er zijn te veel velden aangepast. Gebruik de verhuis opties indien er sprake is van een nieuw adres.")
        };

        int NumberOfChangedProperties() =>
            (straatnaam == Straatnaam ? 0 : 1) +
            (huisnummer == Huisnummer ? 0 : 1) +
            (postcode == Postcode ? 0 : 1) +
            (woonplaats == Woonplaats ? 0 : 1) +
            (land.Equals(Land) ? 0 : 1);

        static ValidationError CreateValidationError(string message) => new(nameof(CorrigeerAdres), message);
    }
   
    private Adres UpdateFields(string straatnaam, string huisnummer, string postcode, string woonplaats, Option<string> land)
    {
        var origineel = GetAdresOnTwoLines();
        Straatnaam = straatnaam.EnsureNotEmpty();
        Huisnummer = huisnummer.EnsureNotEmpty();
        Postcode = postcode.EnsureNotEmpty();
        Woonplaats = woonplaats.EnsureNotEmpty();
        _land = land.AsNullable();
            
        _mutaties.Add(new AdresMutatie(
            adres: this, 
            type: AdresMutatieType.Correctie,
            oldValue: origineel,
            newValue: GetAdresOnTwoLines()
        ));

        return this;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Adres() { /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}