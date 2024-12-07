using DA.Anubis.Domain.Common;
using DA.Anubis.Domain.Contract.AggregateKeys;

namespace DA.Anubis.Domain.LedenAggregate;

public sealed class Notitie : OrdinalEntity<NotitieId>
{
    /// <summary>
    /// Het lid waarvan dit telefoonnummer is.
    /// </summary>
    public LidId LidId { get; }

    /// <summary>
    /// Het eigenlijke telefoonnummer.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value;
    }
    
    public Notitie(Lid lid, string value)
    {
        LidId = lid.Id;
        Value = value;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Notitie() { /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}