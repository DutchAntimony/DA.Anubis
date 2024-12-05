namespace DA.Anubis.Domain.Contract.Enumerations;

/// <summary>
/// Enumeratie met de mogelijke redenen waarom een lid uitgeschreven wordt.
/// </summary>
public enum Uitschrijfreden
{
    Overleden = 1,
    Opzegging = 10,
    GeenBetalendLid = 11,
    Overschrijving = 12,
    Wanbetaling = 20
}