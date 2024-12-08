using Bogus;
using Bogus.DataSets;
using DA.Anubis.Domain.AdresAggregate;
using DA.Anubis.Domain.BetaalmethodeAggregate;
using DA.Anubis.Domain.Contract.AggregateKeys;
using DA.Anubis.Domain.Contract.Enumerations;
using DA.Anubis.Domain.LedenAggregate;
using DA.DDD.CoreLibrary.Extensions;
using DA.DDD.CoreLibrary.ServiceDefinitions;
using DA.Options;
using Microsoft.Extensions.Logging;

namespace DA.Anubis.Tests.CreateDatabase;

internal class RandomDataInserter(
    ILogger<RandomDataInserter> logger,
    double lidProbabilityAfterFirst = 0.65,
    double notitieProbability = 0.3,
    double emailadresProbability = 0.5,
    double telefoonnummerProbability = 0.3,
    double incassoProbability = 0.8,
    double newIncassoProbability = 0.2)
{
    private readonly Faker<Personalia> _personaliaFaker = InitializePersonaliaFaker();
    private readonly Faker<Adres> _adresFaker = InitializeAdresFaker();
    private readonly Faker<Bankrekening> _bankrekeningFaker = InitializeBankrekeningFaker();
    private readonly Faker<IncassoMandaat> _mandaatFaker = InitializeMandaatFaker();
    private readonly Faker _faker = new("nl");

    private static Faker<IncassoMandaat> InitializeMandaatFaker()
    {
        return new Faker<IncassoMandaat>("nl").CustomInstantiator(f => new IncassoMandaat(
            type: f.Random.Enum<MandaatType>(),
            rekeningnummer: f.Finance.Iban(true, "nl"),
            tekendatum: f.Date.PastDateOnly(20),
            tekenInformatie: f.Person.FullName
        ));
    }

    public void GenerateAndInsert(IDbContext context,
        int adressen = 100)
    {
        logger.LogInformation("Start generating adressen.");
        var lidnummer = 1;
        for (var adresCount = 1; adresCount <= adressen; adresCount++)
        {
            var adres = _adresFaker.Generate();
            context.Insert(adres);
            logger.LogInformation("{index} - Adres {straat} {huisnummer}, {postcode} {woonplaats} toegevoegd",
                adresCount, adres.Straatnaam, adres.Huisnummer, adres.Postcode, adres.Woonplaats);
            var betaalmethodes = new List<Betaalmethode>();
            var incasso = IncassoBetaalmethode.Create(_bankrekeningFaker.Generate(), _mandaatFaker.Generate());
            betaalmethodes.Add(incasso);
            
            var hasVolwassenLid = false;
            var leden = new List<Lid>();
            
            while (!hasVolwassenLid || _faker.Random.Double() < lidProbabilityAfterFirst)
            {
                var lid = GenerateNewLid(betaalmethodes, adres, ref hasVolwassenLid, ref lidnummer);
                logger.LogInformation("{index}.{lidnummer} - Lid {voorletters}. {achternaam} toegevoegd aan adres.",
                    adresCount, lid.Lidnummer, lid.Personalia.Voorletters, lid.Personalia.Achternaam);
                leden.Add(lid);
            }

            context.InsertRange(betaalmethodes.Where(methode =>
                leden.Any(l => l.BetaalmethodeId.TryGetValue(out var id) && id.Equals(methode.Id))));

            context.InsertRange(leden);

            var hoofdbewoner = leden.OrderBy(l => l.Personalia.Geboortedatum).First();
            adres.SetOrUpdateHoofdbewoner(hoofdbewoner.Id);
            logger.LogInformation("{index}.{lidnummer} opgeboekt als hoofdbewoner.",
                adresCount, hoofdbewoner.Lidnummer);
        }
    }

    private Lid GenerateNewLid(List<Betaalmethode> betaalmethodes, Adres adres, ref bool hasVolwassenLid, ref int lidnummer)
    {
        var personalia = _personaliaFaker.Generate();
        var age = personalia.Geboortedatum.GetAge(DateTime.Today);
        hasVolwassenLid |= age > 18;
                
        ValueOption<BetaalmethodeId> betaalmethodeId = Option.None;
        if (age > 18)
        {
            betaalmethodeId = GetBetaalmethodeIdForLid(betaalmethodes);
        }
                
        var lid = Lid.Create(lidnummer++, personalia, adres.Id, betaalmethodeId);
        // add notities, telefoonnummers en emailadressen.
        while (_faker.Random.Double() < notitieProbability)
        {
            var notitie = new Notitie(lid, _faker.Lorem.Lines());
            lid.Notities.Append(notitie);
        }
        
        while (_faker.Random.Double() < emailadresProbability)
        {
            var email = new Emailadres(lid, _faker.Person.Email);
            lid.Emailadressen.Append(email);
        }

        while (_faker.Random.Double() < telefoonnummerProbability)
        {
            var telefoon = new Telefoonnummer(lid, _faker.Person.Phone);
            lid.Telefoonnummers.Append(telefoon);
        }
        
        return lid;
    }

    private ValueOption<BetaalmethodeId> GetBetaalmethodeIdForLid(List<Betaalmethode> betaalmethodes)
    {
        ValueOption<BetaalmethodeId> betaalmethodeId;
        if (_faker.Random.Double() < incassoProbability)
        {
            betaalmethodeId = betaalmethodes.First().Id;
        }
        else if (_faker.Random.Double() < newIncassoProbability)
        {
            var incassoNew = IncassoBetaalmethode.Create(_bankrekeningFaker.Generate(), _mandaatFaker.Generate());
            betaalmethodes.Add(incassoNew);
            betaalmethodeId = incassoNew.Id;
        }
        else
        {
            var notaNew = NotaBetaalmethode.Create();
            betaalmethodes.Add(notaNew);
            betaalmethodeId = notaNew.Id;
        }

        return betaalmethodeId;
    }
    
    private static Faker<Bankrekening> InitializeBankrekeningFaker()
    {
        return new Faker<Bankrekening>("nl").CustomInstantiator(f => new Bankrekening(
            iban: new Iban(f.Finance.Iban(false, "nl")),
            bic: f.Finance.Bic(),
            tenNameVan: f.Person.FullName
        ));
    }

    private static Faker<Adres> InitializeAdresFaker()
    {
        return new Faker<Adres>("nl").CustomInstantiator(f =>
        {
            var adres = f.Address;
            return Adres.Create(
                straatnaam: adres.StreetAddress(),
                huisnummer: $"{f.Random.Int(1, 120).ToString()}{f.Random.AlphaNumeric(1).OrNull(f, 0.7f)}",
                postcode: f.Address.ZipCode(),
                woonplaats: f.Address.City(),
                land: Option.None);
        });
    }

    private static Faker<Personalia> InitializePersonaliaFaker()
    {
        return new Faker<Personalia>("nl").CustomInstantiator(f =>
        {
            var fakePerson = f.Person;
            return new Personalia(
                voorletters: fakePerson.FirstName[0].ToString() + ".",
                tussenvoegsel: Option.None,
                achternaam: fakePerson.LastName,
                geslacht: fakePerson.Gender == Name.Gender.Female ? Geslacht.Vrouw : Geslacht.Man,
                geboortedatum: DateOnly.FromDateTime(fakePerson.DateOfBirth)
            );
        });
    }
}