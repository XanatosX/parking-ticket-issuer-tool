namespace ParkingTicketIssuerToolFramework.Features.Shared;

/// <summary>
/// Interface for formatting dates.
/// This interface is used to format dates in a consistent way.
/// </summary>
public interface IDateFormatter
{
    /// <summary>
    /// Formats the date.
    /// </summary>
    /// <param name="date">The date which should be formatted</param>
    /// <returns>The formatted date</returns>
    string FormatDate(DateOnly date);
}