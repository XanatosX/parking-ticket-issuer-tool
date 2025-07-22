using Microsoft.Extensions.DependencyInjection;

namespace ParkingTicketIssuerToolFramework.Features.ParkingTicket;

/// <summary>
/// Dependency injection for the parking ticket feature.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the services required by the parking ticket feature.
    /// This method adds the CreateTicketPdfService to the service collection.
    /// </summary>
    /// <param name="services">The current service collection to use</param>
    /// <returns>The service collection with the parking ticket feature added</returns>
    public static IServiceCollection AddParkingTicketFeature(this IServiceCollection services)
    {
        return services.AddSingleton<CreateTicketPdfService>();
    }
}