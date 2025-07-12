namespace  ParkingTicketIssuerToolFramework;

using Microsoft.Extensions.DependencyInjection;
using ParkingTicketIssuerToolFramework.Features.ParkingTicket;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the services required by the parking ticket issuer tool.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    public static IServiceCollection RegisterFramework(this IServiceCollection services)
    {
        SetupQuestPDF();
        return services.AddParkingTicketFeature();
    }

    private static void SetupQuestPDF()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        QuestPDF.Settings.UseEnvironmentFonts = false;
        QuestPDF.Settings.FontDiscoveryPaths.Add("Resources/Fonts");
        FontManager.RegisterFontFromEmbeddedResource("ParkingTicketIssuerToolFramework.Resources.Fonts.Mechanical.otf");
        FontManager.RegisterFontFromEmbeddedResource("ParkingTicketIssuerToolFramework.Resources.Fonts.MechanicalBold.otf");
        FontManager.RegisterFontFromEmbeddedResource("ParkingTicketIssuerToolFramework.Resources.Fonts.MechanicalBoldCondensed.otf");
    }
}