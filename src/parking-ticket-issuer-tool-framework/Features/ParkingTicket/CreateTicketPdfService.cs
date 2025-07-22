using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Markdown;
using ParkingTicketIssuerToolFramework.Entities;

namespace ParkingTicketIssuerToolFramework.Features.ParkingTicket;

/// <summary>
/// Service to create and save parking tickets as PDF documents.
/// This service uses QuestPDF to generate the PDF and save it to a specified path.
/// </summary>
public class CreateTicketPdfService
{
    private readonly ILogger logger;

    public CreateTicketPdfService(ILogger logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Creates a PDF document for the given parking ticket.
    /// </summary>
    /// <param name="ticket">The parking ticket to create a document for</param>
    /// <returns>The document if any was created or null if something went wrong</returns>
    public Document? createTicketPdf(ParkingTicketData ticket)
    {
        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Mechanical"));

                page.Header().Column(x =>
                {
                    x.Item().Row(row =>
                    {

                        row.RelativeItem().Text("Parking Ticket").SemiBold().FontSize(24).AlignLeft();
                        if (File.Exists(ticket.logoPath))
                        {
                            row.AutoItem().Width(50, Unit.Millimetre).Height(25, Unit.Millimetre).Image(ticket.logoPath).FitArea();
                        }

                    });

                });
                page.Content().PaddingVertical(1, Unit.Centimetre).Column(x =>
                {
                    x.Spacing(20);

                    string vehicle = ticket.usedVehicleName ?? "unknown";
                    x.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(75);
                            columns.ConstantColumn(150);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("");
                            header.Cell().Text("");
                        });
                        table.Cell().Text("Issuer:");
                        table.Cell().Text(ticket.issuingOfficer);

                        table.Cell().Text("Driver:");
                        table.Cell().Text(ticket.driverName);


                        table.Cell().Text("Vehicle:");
                        table.Cell().Text(vehicle);

                        table.Cell().Text("Date:");
                        table.Cell().Text(ticket.issueDate.ToString());
                    });
                    string location = ticket.location ?? "unknown location";
                    x.Item().Markdown($"Officer **{ticket.issuingOfficer}** issued a parking ticket to **{ticket.driverName}** on **{ticket.issueDate}** for the vehicle **{vehicle}** at **{location}**");
                    x.Item().Text("Sentence").Bold();
                    x.Item().Text(ticket.sentence);
                    if (ticket.additionalInformation != null)
                    {
                        x.Item().Text("Additional Information").Bold();
                        x.Item().Text(ticket.additionalInformation);
                    }
                    if (File.Exists(ticket.evidencePath))
                    {
                        x.Item().Text("Please check the next page for evidence").AlignCenter().Bold();
                        x.Item().PageBreak();
                        x.Item().Image(ticket.evidencePath).FitArea();
                    }
                });

                page.Footer().AlignCenter().Markdown($"Issued by **{ticket.issuingOfficer}** on **{ticket.issueDate}**");
            });
        });
    }

    /// <summary>
    /// Creates a PDF document for the given parking ticket and saves it to the specified path.
    /// </summary>
    /// <param name="ticket">The ticket data to create a document for</param>
    /// <param name="path">The path to save the result document o</param>
    /// <returns>The path to the PDF document if any was created, null if something gone wrong</returns>
    public string? createAndSaveTicketPdf(ParkingTicketData ticket, string path)
    {
        try
        {
            createTicketPdf(ticket)?.GeneratePdf(path);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create or save the parking ticket PDF.");
            return null;
        }
        return File.Exists(path) ? path : null;
    }
}