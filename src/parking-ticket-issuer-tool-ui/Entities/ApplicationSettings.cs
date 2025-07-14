using System;
using System.Collections.Generic;

namespace ParkingTicketIssuerToolUI.Entities;

public class ApplicationSettings
{
    public string LogoPath { get; set; } = string.Empty;
    public List<string> IssuingOfficers { get; set; } = new List<string>();
    public List<string> DriverNames { get; set; } = new List<string>();
    public List<string> Locations { get; set; } = new List<string>();
    public List<string> Sentences { get; set; } = new List<string>();
    public List<string> VehicleNames { get; set; } = new List<string>();
    
    public string LastUsedOfficerName { get; set; } = string.Empty;
}