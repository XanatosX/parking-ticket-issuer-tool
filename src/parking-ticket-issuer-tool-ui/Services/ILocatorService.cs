
using System;

namespace ParkingTicketIssuerToolUI.Services;

public interface ILocatorService
{
    T? GetService<T>();
    T? GetService<T>(Action<T?>? furtherInitialization);
    T? GetService<T>(Func<IServiceProvider, T?> objectFactory);
    object? GetService(Type typeOf);
    T?[] GetServices<T>();
    object?[] GetServices(Type typeOf);
}