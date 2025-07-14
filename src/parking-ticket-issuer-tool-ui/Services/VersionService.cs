using System.Reflection;

namespace ParkingTicketIssuerToolUI.Services;

public sealed class VersionService
{
    public string GetVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;
        return $"{version?.Major}.{version?.Minor}.{version?.Build}";
    }
}