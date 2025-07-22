using System;
using System.Collections.Generic;
using System.Linq;
using ParkingTicketIssuerToolFramework.Features.Shared;
using ParkingTicketIssuerToolUI.Entities;

namespace ParkingTicketIssuerToolUI.Services;

public class CustomDateFormat : IDateFormatter
{
    private readonly SettingsService settingsService;
    private readonly IConfigSerializer<IEnumerable<DateFormatConfig>> dateFormats;

    public CustomDateFormat(SettingsService settingsService, IConfigSerializer<IEnumerable<DateFormatConfig>> dateFormats)
    {
        this.settingsService = settingsService;
        this.dateFormats = dateFormats;
    }

    public string FormatDate(DateOnly date)
    {
        var allFormats = dateFormats.Deserialize();
        if (allFormats == null)
        {
            return date.ToString();
        }
        var settings = settingsService.GetSettings();
        var formatName = settings.FormatName;
        var format = allFormats.FirstOrDefault(x => x.Id == formatName, new DateFormatConfig("", "{DAY}.{MONTH}.{YEAR}", "", ""));
        var returnFormat = format.Format;
        returnFormat = returnFormat.Replace("{DAY}", date.Day.ToString("00"))
                                   .Replace("{MONTH}", date.Month.ToString("00"))
                                   .Replace("{YEAR}", date.Year.ToString("0000"));
        return returnFormat;
    }
}