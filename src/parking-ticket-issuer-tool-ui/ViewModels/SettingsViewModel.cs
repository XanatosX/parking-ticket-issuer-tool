using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ParkingTicketIssuerToolUI.Entities.Messages;
using ParkingTicketIssuerToolUI.Messages;
using ParkingTicketIssuerToolUI.Services;

namespace ParkingTicketIssuerToolUI.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly SettingsService settingsService;

    [ObservableProperty]
    [Required(ErrorMessage = "Logo path is required.")]
    [CustomValidation(typeof(SettingsViewModel), nameof(ValidateImage))]
    [NotifyCanExecuteChangedFor(nameof(SaveSettingsCommand))]
    private string logoPath;

    private bool isLogoValid;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ClearAllCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearOfficerNamesCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearDriverNamesCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearLocationsCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearSentencesCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearVehicleNamesCommand))]
    private bool updateClearButtons = true;

    public SettingsViewModel(SettingsService settingsService)
    {
        LogoPath = settingsService.GetSettings().LogoPath;
        this.settingsService = settingsService;
    }

    [RelayCommand(CanExecute = nameof(CanSaveSettings))]
    private void SaveSettings()
    {
        settingsService.UpdateSettings(settings =>
        {
            settings.LogoPath = LogoPath;
        });

        WeakReferenceMessenger.Default.Send(new SettingsChangedMessage(settingsService.GetSettings()));
    }

    private bool CanSaveSettings()
    {
        return !string.IsNullOrWhiteSpace(LogoPath) && isLogoValid;
    }

    [RelayCommand(CanExecute = nameof(CanClearAll))]
    private void ClearAll()
    {
        settingsService.UpdateSettings(settings =>
        {
            settings.IssuingOfficers.Clear();
            settings.LastUsedOfficerName = string.Empty;
            settings.Locations.Clear();
            settings.Sentences.Clear();
            settings.VehicleNames.Clear();
        });
        UpdateClearButtons = !UpdateClearButtons;
    }

    private bool CanClearAll()
    {
        return CanClearDriverNames() ||
               CanClearLocations() ||
               CanClearSentences() ||
               CanClearVehicleNames() ||
               CanClearOfficerNames();
    }

    [RelayCommand(CanExecute = nameof(CanClearOfficerNames))]
    private void ClearOfficerNames()
    {
        settingsService.UpdateSettings(settings =>
        {
            settings.IssuingOfficers.Clear();
            settings.LastUsedOfficerName = string.Empty;
        });

        WeakReferenceMessenger.Default.Send(new SettingsChangedMessage(settingsService.GetSettings()));
        UpdateClearButtons = !UpdateClearButtons;
    }

    private bool CanClearOfficerNames()
    {
        var settings = settingsService.GetSettings();
        return settings.IssuingOfficers.Count > 0;
    }

    [RelayCommand(CanExecute = nameof(CanClearDriverNames))]
    private void ClearDriverNames()
    {
        settingsService.UpdateSettings(settings =>
        {
            settings.DriverNames.Clear();
        });

        WeakReferenceMessenger.Default.Send(new SettingsChangedMessage(settingsService.GetSettings()));
        UpdateClearButtons = !UpdateClearButtons;
    }

    private bool CanClearDriverNames()
    {
        var settings = settingsService.GetSettings();
        return settings.DriverNames.Count > 0;
    }

    [RelayCommand(CanExecute = nameof(CanClearLocations))]
    private void ClearLocations()
    {
        settingsService.UpdateSettings(settings =>
        {
            settings.Locations.Clear();
        });

        WeakReferenceMessenger.Default.Send(new SettingsChangedMessage(settingsService.GetSettings()));
        UpdateClearButtons = !UpdateClearButtons;
    }

    private bool CanClearLocations()
    {
        var settings = settingsService.GetSettings();
        return settings.Locations.Count > 0;
    }

    [RelayCommand(CanExecute = nameof(CanClearSentences))]
    private void ClearSentences()
    {
        settingsService.UpdateSettings(settings =>
        {
            settings.Sentences.Clear();
        });

        WeakReferenceMessenger.Default.Send(new SettingsChangedMessage(settingsService.GetSettings()));
        UpdateClearButtons = !UpdateClearButtons;
    }

    private bool CanClearSentences()
    {
        var settings = settingsService.GetSettings();
        return settings.Sentences.Count > 0;
    }

    [RelayCommand(CanExecute = nameof(CanClearVehicleNames))]
    private void ClearVehicleNames()
    {
        settingsService.UpdateSettings(settings =>
        {
            settings.VehicleNames.Clear();
        });

        WeakReferenceMessenger.Default.Send(new SettingsChangedMessage(settingsService.GetSettings()));
        UpdateClearButtons = !UpdateClearButtons;
    }

    private bool CanClearVehicleNames()
    {
        var settings = settingsService.GetSettings();
        return settings.VehicleNames.Count > 0;
    }

    [RelayCommand]
    private async Task OpenLogoFile()
    {
        var files = await WeakReferenceMessenger.Default.Send(new OpenFileRequestMessage
        {
            AllowMultiple = false,
            Title = "Select Logo File",
            Filters = new List<FilePickerFileType>
            {
                new FilePickerFileType("Image Files")
                {
                    Patterns =  new List<string> { "*.png", "*.jpg", "*.jpeg" }
                }
            }
        });

        if (files.Count > 0)
        {
            var file = files[0];
            LogoPath = file;
        }
    }

    public static ValidationResult ValidateImage(string path, ValidationContext context)
    {
        Bitmap? bitmap = null;
        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            try
            {
                bitmap = new Bitmap(stream);
            }
            catch (Exception)
            {
                // Handle exceptions related to invalid image formats
                return new("Could not load image. Please select a valid image file.");
            }
        }
        var isValid = bitmap.Size.Width == bitmap.Size.Height && bitmap.Size.Width <= 250;
        if (context.ObjectInstance is SettingsViewModel viewModel)
        {
            viewModel.isLogoValid = false;
            if (isValid )
            {
                viewModel.isLogoValid = true;
            }
        }
        return isValid ? ValidationResult.Success : new("Logo must be a square image with a maximum size of 250x250 pixels.");
    }
}