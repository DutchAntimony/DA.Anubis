using DA.Anubis.Domain.Common;
using DA.Anubis.Domain.Contract.AggregateKeys;

namespace DA.Anubis.Domain.LedenAggregate;

public sealed class Emailadres : OrdinalEntity<EmailadresId>
{
    /// <summary>
    /// Het lid waarvan dit emailadres is.
    /// </summary>
    public LidId LidId { get; }

    /// <summary>
    /// Het eigenlijke emailadres.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Value;
    }
    
    public Emailadres(Lid lid, string value)
    {
        LidId = lid.Id;
        Value = value;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Emailadres() { /* empty constructor for ORM */ }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}