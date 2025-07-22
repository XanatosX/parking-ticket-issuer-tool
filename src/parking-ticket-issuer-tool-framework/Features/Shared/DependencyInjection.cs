using Microsoft.Extensions.DependencyInjection;

namespace ParkingTicketIssuerToolFramework.Features.Shared;

/// <summary>
/// Dependency injection for the shared features.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the shared dependencies to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the data to</param>
    /// <returns>The altered service collection</returns>
    public static IServiceCollection AddSharedDependencies(this IServiceCollection services)
    {
        return services.AddSingleton<IDateFormatter, DefaultDateFormatter>();
    }
}