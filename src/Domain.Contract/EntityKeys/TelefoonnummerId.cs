using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.Contract.EntityKeys;

/// <summary>
/// Uniek identifier van een telefoonnummer.
/// </summary>
public record struct TelefoonnummerId : IEntityKey
{
    /// <inheritdoc />
    public Guid Value { get; init; }
}