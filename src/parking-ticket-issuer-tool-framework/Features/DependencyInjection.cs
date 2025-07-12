using Microsoft.Extensions.DependencyInjection;
using ParkingTicketIssuerToolFramework.Features.ParkingTicket;

namespace ParkingTicketIssuerToolFramework.Features;

/// <summary>
/// Dependency injection for the parking ticket issuer tool features.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the services required by the parking ticket issuer tool.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    public static IServiceCollection RegisterFeatures(this IServiceCollection services)
    {
        return services.AddParkingTicketFeature();
    }
}