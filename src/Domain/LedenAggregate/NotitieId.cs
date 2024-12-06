using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.LedenAggregate;

/// <summary>
/// Uniek identifier van een notitie.
/// </summary>
public record struct NotitieId : IEntityKey
{
    /// <inheritdoc />
    public Guid Value { get; init; }
}