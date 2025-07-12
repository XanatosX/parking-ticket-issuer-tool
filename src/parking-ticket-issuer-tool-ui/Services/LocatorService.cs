using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ParkingTicketIssuerToolUI.Services;


internal sealed class MicrosoftLocatorService : ILocatorService
{
    private readonly IServiceProvider serviceProvider;

    public MicrosoftLocatorService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <inheritdoc/>
    public T?[] GetServices<T>()
    {
        return serviceProvider.GetServices<T>().ToArray();
    }

    /// <inheritdoc/>
    public object?[] GetServices(Type typeOf)
    {
        return serviceProvider.GetServices(typeOf).ToArray();
    }

    /// <inheritdoc/>
    public T? GetService<T>()
    {
        return GetService<T>(furtherInitialization: null);
    }

    /// <inheritdoc/>
    public T? GetService<T>(Action<T?>? furtherInitialization)
    {
        T? returnData = serviceProvider.GetService<T>() ?? ActivatorUtilities.CreateInstance<T>(serviceProvider);
        if (furtherInitialization is not null)
        {
            furtherInitialization(returnData);
        }
        return returnData;
    }

    /// <inheritdoc/>
    public T? GetService<T>(Func<IServiceProvider, T?> objectFactory)
    {
        if (objectFactory is null)
        {
            return default;
        }
        return objectFactory.Invoke(serviceProvider);
    }

    /// <inheritdoc/>
    public object? GetService(Type typeOf)
    {
        return serviceProvider.GetService(typeOf) ?? ActivatorUtilities.CreateInstance(serviceProvider, typeOf);
    }
}