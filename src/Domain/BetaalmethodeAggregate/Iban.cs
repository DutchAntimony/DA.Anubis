using System.Text.RegularExpressions;
using DA.Results;
using DA.Results.Extensions;
using DA.Results.Issues;

namespace DA.Anubis.Domain.BetaalmethodeAggregate;

public readonly record struct Iban
{
    public string Landcode { get; }
    public string ControleGetal { get; }
    public string BankIdentifier { get; }
    public string Rekeningnummer { get; }

    public override string ToString() =>
        $"{Landcode} {ControleGetal} {BankIdentifier} {Regex.Replace(Rekeningnummer, ".{4}", "$0 ").Trim()}";

    public string ToFlatString() =>
        $"{Landcode}{ControleGetal}{BankIdentifier}{Rekeningnummer}";

    private Iban(string value)
    {
        value = value.Replace(" ", "").ToUpperInvariant();
        Landcode = value[..2];
        ControleGetal = value.Substring(2, 2);
        BankIdentifier = value.Substring(4, 4);
        Rekeningnummer = value[8..];
    }

    public static Result<Iban> TryCreate(string value)
    {
        return IsValidIban(value).Map(new Iban(value));
    }

    private static NoContentResult IsValidIban(string iban)
    {
        if (iban.Length is < 15 or > 34)
        {
            return new ValidationError(nameof(Iban), "Ongeldige iban. Lengte is ongeldig.");
        }

        if (!Regex.IsMatch(iban, "^[A-Z]{2}$"))
        {
            return new ValidationError(nameof(Iban), "Ongeldige iban. Controleer de invoer.");
        }
        
        // Rearrange IBAN: move the first four characters to the end
        var rearranged = string.Concat(iban.AsSpan(4), iban.AsSpan(0, 4));

        // Convert letters to numbers (A = 10, B = 11, ..., Z = 35)
        var numericIban = "";
        foreach (var c in rearranged)
        {
            if (char.IsLetter(c))
                numericIban += (c - 'A' + 10).ToString();
            else
                numericIban += c;
        }

        // Perform mod-97 operation on the numeric IBAN
        return Mod97(numericIban) == 1
            ? Result.Ok()
            : new ValidationError(nameof(Iban), "Ongeldige iban. Controleer de invoer.");
    }

    private static int Mod97(string numericIban)
    {
        // Use a sliding window to perform mod-97, as the IBAN number can be very large
        var remainder = 0;
        foreach (var c in numericIban)
        {
            remainder = (remainder * 10 + (c - '0')) % 97;
        }
        return remainder;
    }
}