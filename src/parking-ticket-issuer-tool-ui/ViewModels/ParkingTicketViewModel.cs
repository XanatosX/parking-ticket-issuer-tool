using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ParkingTicketIssuerToolFramework.Entities;
using ParkingTicketIssuerToolUI.Entities.Messages;
using ParkingTicketIssuerToolUI.Services;

namespace ParkingTicketIssuerToolUI.ViewModels;

public partial class ParkingTicketViewModel : ViewModelBase
{
    private readonly CreateTicketPdfService createTicketPdfService;
    private readonly SettingsService settingsService;
    [ObservableProperty]
    [Required(ErrorMessage = "Issuing officer name is required.")]
    [MinLength(3, ErrorMessage = "Issuing officer name must be at least 3 characters long.")]
    [NotifyCanExecuteChangedFor(nameof(CreateTicketCommand))]
    private string? issuingOfficerName;

    [ObservableProperty]
    [Required(ErrorMessage = "Driver name is required.")]
    [MinLength(3, ErrorMessage = "Driver name must be at least 3 characters long.")]
    [NotifyCanExecuteChangedFor(nameof(CreateTicketCommand))]
    private string? driverName;

    [ObservableProperty]
    
    [Required(ErrorMessage = "Sentence is required.")]
    [MaxLength(100, ErrorMessage = "Sentence must be at most 100 characters long.")]
    [NotifyCanExecuteChangedFor(nameof(CreateTicketCommand))]
    private string? sentence;

    [ObservableProperty]
    [Required(ErrorMessage = "Evidence file is required.")]
    [NotifyCanExecuteChangedFor(nameof(CreateTicketCommand))]
    private string? evidencePath;

    [ObservableProperty]
    private string? usedVehicleName;

    [ObservableProperty]
    private string? location;

    [ObservableProperty]
    private string? additionalInformation;

    [ObservableProperty]
    private List<string>? issuingOfficers;

    [ObservableProperty]
    private List<string>? possibleSentences;

    [ObservableProperty]
    private List<string>? locations;

    [ObservableProperty]
    private List<string>? driverNames;

    [ObservableProperty]
    private List<string>? vehicleNames;

    public ParkingTicketViewModel(CreateTicketPdfService createTicketPdfService, SettingsService settingsService)
    {
        this.createTicketPdfService = createTicketPdfService;
        this.settingsService = settingsService;
        RefillAutofillList();
    }

    private void RefillAutofillList()
    {
        var settings = settingsService.GetSettings();
        IssuingOfficerName = settings.LastUsedOfficerName;
        IssuingOfficers = settings.IssuingOfficers.Where(entry => !string.IsNullOrEmpty(entry))
                                                  .OfType<string>()
                                                  .Order()
                                                  .ToList();

        DriverNames = settings.DriverNames.Where(entry => !string.IsNullOrEmpty(entry))
                                          .OfType<string>()
                                          .Order()
                                          .ToList();
                                          
        VehicleNames = settings.VehicleNames.Where(entry => !string.IsNullOrEmpty(entry))
                                            .OfType<string>()
                                            .Order()
                                            .ToList();

        Locations = settings.Locations.Where(entry => !string.IsNullOrEmpty(entry))
                                      .OfType<string>()
                                      .Order()
                                      .ToList();

        PossibleSentences = settings.Sentences.Where(entry => !string.IsNullOrEmpty(entry))
                                              .OfType<string>()
                                              .Order()
                                              .ToList();
    }

    [RelayCommand]
    private async Task SelectEvidenceFile()
    {
        var result = await WeakReferenceMessenger.Default.Send(new OpenFileRequestMessage()
        {
            AllowMultiple = false,
            Title = "Select Evidence File",
            Filters = new List<FilePickerFileType>
            {
                new FilePickerFileType("Image Files")
                {
                    Patterns = new List<string> { "*.jpg", "*.jpeg", "*.png" }
                },
            }
        });
        if (result != null && result.Count > 0)
        {
            EvidencePath = result[0];
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecuteCreateTicket))]
    private async Task CreateTicket()
    {
        var path = await WeakReferenceMessenger.Default.Send(new SaveFileRequestMessage()
        {
            Title = "Save Parking Ticket",
            DefaultFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{IssuingOfficerName}_{DriverName}.pdf",
            Filters = new List<FilePickerFileType>
            {
                new FilePickerFileType("PDF Files")
                {
                    Patterns = new List<string> { "*.pdf" }
                },
            }
        });
        var settings = settingsService.GetSettings();
        var data = new ParkingTicket(IssuingOfficerName ?? string.Empty,
                                     DriverName ?? string.Empty,
                                     Sentence ?? string.Empty,
                                     EvidencePath ?? string.Empty,
                                     DateOnly.FromDateTime(DateTime.Now),
                                     settings.LogoPath,
                                     UsedVehicleName,
                                     Location,
                                     AdditionalInformation);

        settingsService.UpdateSettings(data =>
        {
            data.IssuingOfficers.Add(IssuingOfficerName ?? string.Empty);
            data.IssuingOfficers = data.IssuingOfficers.Distinct().ToList();
            data.LastUsedOfficerName = IssuingOfficerName ?? string.Empty;

            data.DriverNames.Add(DriverName ?? string.Empty);
            data.DriverNames = data.DriverNames.Distinct().ToList();

            data.Sentences.Add(Sentence ?? string.Empty);
            data.Sentences = data.Sentences.Distinct().ToList();

            if (Location != null && !string.IsNullOrWhiteSpace(Location))
            {
                data.Locations.Add(Location);
                data.Locations = data.Locations.Distinct().ToList();
            }

            if (UsedVehicleName != null && !string.IsNullOrWhiteSpace(UsedVehicleName))
            {
                data.VehicleNames.Add(UsedVehicleName);
                data.VehicleNames = data.VehicleNames.Distinct().ToList();
            }
        });

        RefillAutofillList();

        createTicketPdfService.createAndSaveTicketPdf(data, path);
    }

    private bool CanExecuteCreateTicket()
    {
        return !string.IsNullOrWhiteSpace(IssuingOfficerName) &&
               !string.IsNullOrWhiteSpace(DriverName) &&
               !string.IsNullOrWhiteSpace(Sentence) &&
               !string.IsNullOrWhiteSpace(EvidencePath) &&
               File.Exists(EvidencePath);
    }
}