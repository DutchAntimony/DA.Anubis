using DA.Anubis.Domain.BetaalmethodeAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DA.Anubis.Persistence.Converters;

/// <summary>
/// We slaan alleen de string van de iban op en maken een nieuwe iban van de string bij deserialisatie.
/// </summary>
public class IbanConverter() : ValueConverter<Iban, string>(
    iban => iban.ToString(),
    str => new Iban(str));