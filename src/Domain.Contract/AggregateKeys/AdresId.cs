using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.Contract.AggregateKeys;

/// <summary>
/// Uniek identifier van een Adres.
/// </summary>
public record struct AdresId : IEntityKey
{
    /// <inheritdoc />
    public Guid Value { get; init; }

    public static AdresId FromValue(Guid value)
    {
        return new AdresId { Value = value };
    }
}