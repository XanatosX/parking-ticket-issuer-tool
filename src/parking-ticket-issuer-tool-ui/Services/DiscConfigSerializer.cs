using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia.Controls.Platform;
using Microsoft.Extensions.Logging;
using ParkingTicketIssuerTool.Services;

namespace ParkingTicketIssuerToolUI.Services;

public class DiscConfigSerializer : IConfigSerializer<IEnumerable<DateFormatConfig>>
{
    private IEnumerable<DateFormatConfig> dateFormats = new List<DateFormatConfig>();

    private readonly PathService pathService;
    private readonly ILogger logger;

    public DiscConfigSerializer(PathService pathService, ILogger logger)
    {
        this.pathService = pathService;
        this.logger = logger;
    }

    public IEnumerable<DateFormatConfig>? Deserialize()
    {
        if (dateFormats != null && dateFormats.Any())
        {
            return dateFormats;
        }

        string absolutePath = Path.Combine(pathService.GetResourcesDirectory(), "date-formats.json");
        try
        {

            using (FileStream fs = File.OpenRead(absolutePath))
            {
                dateFormats = JsonSerializer.Deserialize<List<DateFormatConfig>>(fs) ?? Enumerable.Empty<DateFormatConfig>();
            }

        }
        catch (System.Exception ex)
        {
            logger.LogError(ex.Message);
            return Enumerable.Empty<DateFormatConfig>();
        }
        
        return dateFormats;
    }
}