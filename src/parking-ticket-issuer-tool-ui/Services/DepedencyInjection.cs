using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ParkingTicketIssuerTool.Services;
using ParkingTicketIssuerToolFramework.Features.Shared;
using ParkingTicketIssuerToolUI.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddSingleton<ILocatorService, MicrosoftLocatorService>()
                       .AddSingleton<SettingsService>()
                       .AddSingleton<PathService>()
                       .AddSingleton<VersionService>()
                       .AddSingleton<IConfigSerializer<IEnumerable<DateFormatConfig>>, DiscConfigSerializer>()
                       .Replace(new ServiceDescriptor(typeof(IDateFormatter), typeof(CustomDateFormat), ServiceLifetime.Singleton));
    }
}