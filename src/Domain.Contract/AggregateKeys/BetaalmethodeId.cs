using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.Contract.AggregateKeys;

/// <summary>
/// Uniek identifier van een betaalmethode.
/// </summary>
public record struct BetaalmethodeId : IEntityKey
{
    /// <inheritdoc />
    public Guid Value { get; init; }
}