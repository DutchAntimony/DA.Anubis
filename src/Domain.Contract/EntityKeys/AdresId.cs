using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.Contract.EntityKeys;

/// <summary>
/// Uniek identifier van een Adres.
/// </summary>
public record struct AdresId : IEntityKey
{
    /// <inheritdoc />
    public Guid Value { get; init; }
}