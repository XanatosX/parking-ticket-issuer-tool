namespace ParkingTicketIssuerToolFramework.Features.Shared;

/// <summary>
/// Default date formatter. Will simply return the date as a string.
/// </summary>
public class DefaultDateFormatter : IDateFormatter
{
    /// <inheritdoc/>
    public string FormatDate(DateOnly date)
    {
        return date.ToString();
    }
}


