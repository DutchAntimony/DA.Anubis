using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.Contract.EntityKeys;

/// <summary>
/// Uniek identifier van een betaalmethode.
/// </summary>
public record struct BetaalmethodeId : IEntityKey
{
    /// <inheritdoc />
    public Guid Value { get; init; }
}