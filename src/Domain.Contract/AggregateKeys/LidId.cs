using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.Contract.AggregateKeys;

/// <summary>
/// Uniek identifier van een Lid.
/// </summary>
public record struct LidId : IEntityKey
{
    /// <inheritdoc />
    public Guid Value { get; init; }
}