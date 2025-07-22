using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Markdown;
using ParkingTicketIssuerToolFramework.Entities;
using ParkingTicketIssuerToolFramework.Features.Shared;
using ParkingTicketIssuerTool.Features.Shared;
using ParkingTicketIssuerToolFramework.Features.Shared.Enums;

namespace ParkingTicketIssuerToolFramework.Features.ParkingTicket;

/// <summary>
/// Service to create and save parking tickets as PDF documents.
/// This service uses QuestPDF to generate the PDF and save it to a specified path.
/// </summary>
public class CreateTicketPdfService
{
    private readonly ILogger logger;
    private readonly IDateFormatter dateFormatter;
    private readonly ITranslationService translationService;

    public CreateTicketPdfService(ILogger logger,
                                  IDateFormatter dateFormatter,
                                  ITranslationService translationService)
    {
        this.logger = logger;
        this.dateFormatter = dateFormatter;
        this.translationService = translationService;
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
            string date = dateFormatter.FormatDate(ticket.issueDate);
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

                        row.RelativeItem().Text(translationService.Translate(TranslationEnums.PARKING_TICKET)).SemiBold().FontSize(24).AlignLeft();
                        if (File.Exists(ticket.logoPath))
                        {
                            row.AutoItem().Width(50, Unit.Millimetre).Height(25, Unit.Millimetre).Image(ticket.logoPath).FitArea();
                        }

                    });

                });
                page.Content().PaddingVertical(1, Unit.Centimetre).Column(x =>
                {
                    x.Spacing(20);

                    string vehicle = ticket.usedVehicleName ?? translationService.Translate(TranslationEnums.UNKNOWN_VEHICLE);
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
                        table.Cell().Text(translationService.Translate(TranslationEnums.ISSUER));
                        table.Cell().Text(ticket.issuingOfficer);

                        table.Cell().Text(translationService.Translate(TranslationEnums.DRIVER));
                        table.Cell().Text(ticket.driverName);


                        table.Cell().Text(translationService.Translate(TranslationEnums.VEHICLE));
                        table.Cell().Text(vehicle);

                        table.Cell().Text(translationService.Translate(TranslationEnums.DATE));
                        table.Cell().Text(date);
                    });
                    string location = ticket.location ?? translationService.Translate(TranslationEnums.UNKNOWN_LOCATION);
                    string parkingTicketBody = 
                    parkingTicketBody = string.Format(translationService.Translate(TranslationEnums.OFFICER_ISSUED_TICKET),
                                                      ticket.issuingOfficer,
                                                      ticket.driverName,
                                                      date,
                                                      vehicle,
                                                      location);
                    x.Item().Markdown(parkingTicketBody);
                    x.Item().Text(translationService.Translate(TranslationEnums.SENTENCE)).Bold();
                    x.Item().Text(ticket.sentence);
                    if (ticket.additionalInformation != null)
                    {
                        x.Item().Text(translationService.Translate(TranslationEnums.ADDITIONAL_INFORMATION)).Bold();
                        x.Item().Text(ticket.additionalInformation);
                    }
                    if (File.Exists(ticket.evidencePath))
                    {
                        x.Item().Text(translationService.Translate(TranslationEnums.PLEASE_CHECK_NEXT_PAGE)).AlignCenter().Bold();
                        x.Item().PageBreak();
                        x.Item().Image(ticket.evidencePath).FitArea();
                    }
                });

                string issuedBy = string.Format(translationService.Translate(TranslationEnums.ISSUED_BY),
                                                ticket.issuingOfficer,
                                                date);
                page.Footer().AlignCenter().Markdown(issuedBy);
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