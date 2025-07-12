using System;
using System.IO;
using System.Text.Json;
using ParkingTicketIssuerTool.Services;
using ParkingTicketIssuerToolUI.Entities;

namespace ParkingTicketIssuerToolUI.Services;

public class SettingsService
{
    private ApplicationSettings? settings;

    private string settingsFilePath;

    public SettingsService(PathService pathService)
    {
        settingsFilePath = Path.Combine(pathService.GetSettingsDirectory(), "settings.json");
    }

    public ApplicationSettings GetSettings()
    {
        settings = settings ?? LoadSettings();

        return settings;
    }

    private ApplicationSettings LoadSettings()
    {
        ApplicationSettings returnSettings = new ApplicationSettings();
        if (File.Exists(settingsFilePath))
        {
            using (FileStream fs = new FileStream(settingsFilePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    try
                    {
                        returnSettings = JsonSerializer.Deserialize<ApplicationSettings>(reader.BaseStream) ?? new ApplicationSettings();
                    }
                    catch (JsonException)
                    {
                        // No exception for now
                    }
                }
            }
        }
        return returnSettings;
    }

    public bool SaveSettings()
    {
        if (settings == null)
        {
            return false;
        }

        string data = JsonSerializer.Serialize(settings);
        using (FileStream fs = new FileStream(settingsFilePath, FileMode.Create, FileAccess.Write))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(data);
            }
        }
        return File.Exists(settingsFilePath);
    }

    public bool UpdateSettings(Action<ApplicationSettings> updateAction)
    {
        updateAction?.Invoke(GetSettings());
        return SaveSettings();
    }

    
}