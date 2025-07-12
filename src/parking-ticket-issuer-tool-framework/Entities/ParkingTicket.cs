namespace ParkingTicketIssuerToolFramework.Entities;

/// <summary>
/// Represents a parking ticket entity.
/// This record contains information about the issuing officer, driver's name, sentence, an evidence image and the date of issue.
/// </summary>
/// <param name="issuingOfficer">The office which did issue the parking ticket</param>
/// <param name="driverName">The name of the driver issuing the ticket</param>
/// <param name="sentence">The sentence for the driver to do to get rid of the ticket</param>
/// <param name="evidencePath">Path to an image showing evidence of the parking situation</param>
/// <param name="issueDate">The date this ticket was created</param>
/// <param name="usedVehicleName">The Name of the vehicle which was used</param>
/// <param name="location">The location this ticket was created for</param>
/// <param name="additionalInformation">Any additional information</param>
public record ParkingTicket(
    string issuingOfficer,
    string driverName,
    string sentence,
    string evidencePath,
    DateOnly issueDate,
    string? logoPath,
    string? usedVehicleName,
    string? location,
    string? additionalInformation = null
);