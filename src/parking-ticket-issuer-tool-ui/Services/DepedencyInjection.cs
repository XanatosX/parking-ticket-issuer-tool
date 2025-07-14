using Microsoft.Extensions.DependencyInjection;
using ParkingTicketIssuerTool.Services;
using ParkingTicketIssuerToolUI.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddSingleton<ILocatorService, MicrosoftLocatorService>()
                       .AddSingleton<SettingsService>()
                       .AddSingleton<PathService>()
                       .AddSingleton<VersionService>();
    }
}