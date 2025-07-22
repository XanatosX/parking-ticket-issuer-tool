using System;
using System.IO;

namespace ParkingTicketIssuerTool.Services;

public class PathService
{
    public string GetSettingsDirectory()
    {
        string returnPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ParkingTicketIssuerTool");
        if (!Directory.Exists(returnPath))
        {
            Directory.CreateDirectory(returnPath);
        }
        return returnPath;
    }

    public string GetResourcesDirectory()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
    }
}