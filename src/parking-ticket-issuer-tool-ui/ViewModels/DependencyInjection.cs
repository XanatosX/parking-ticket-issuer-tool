using Microsoft.Extensions.DependencyInjection;

namespace ParkingTicketIssuerToolUI.ViewModels;

public static class DependencyInjection
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        return services.AddTransient<MainWindowViewModel>();
    }
}