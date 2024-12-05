using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.Contract.EntityKeys;

/// <summary>
/// Uniek identifier van een emailadres.
/// </summary>
public record struct EmailadresId : IEntityKey
{
    /// <inheritdoc />
    public Guid Value { get; init; }
}