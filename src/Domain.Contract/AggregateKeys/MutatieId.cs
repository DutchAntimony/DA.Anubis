using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.Contract.AggregateKeys;

public record struct MutatieId : IEntityKey
{
    /// <inheritdoc />
    public Guid Value { get; init; }
}