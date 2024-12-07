using DA.DDD.CoreLibrary.Entities;

namespace DA.Anubis.Domain.Common;

/// <summary>
/// Een entity met een positie element.
/// Deze entiteiten hebben een relatieve positie t.o.v. van andere ordinalEntities van dezelfde root.
/// </summary>
/// <typeparam name="TEntityKey"></typeparam>
public abstract class OrdinalEntity<TEntityKey> : Entity<TEntityKey>
    where TEntityKey : IEntityKey, IEquatable<TEntityKey>, new()
{
    /// <summary>
    /// De positie die deze entity inneemt in de collectie.
    /// Deze ordinal is 1-based!
    /// </summary>
    public int Ordinal { get; private set; } = -1;

    internal void SetOrdinal(int ordinal) => Ordinal = ordinal;
    internal void MoveDown() => Ordinal += 1;
    internal void MoveUp() => Ordinal -= 1;
}