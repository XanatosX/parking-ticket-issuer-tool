using System;
using System.Collections.Generic;
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
    private string logoPath;

    public SettingsViewModel(SettingsService settingsService)
    {
        LogoPath = settingsService.GetSettings().LogoPath;
        this.settingsService = settingsService;
    }

    [RelayCommand]
    private void SaveSettings()
    {
        settingsService.UpdateSettings(settings =>
        {
            settings.LogoPath = LogoPath;
        });

        WeakReferenceMessenger.Default.Send(new SettingsChangedMessage(settingsService.GetSettings()));
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
            if (ValidateImage(file))
            {
                LogoPath = file;
            }
        }
    }

    private bool ValidateImage(string path)
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
                return false;
            }
        }

        return bitmap.Size.Width == bitmap.Size.Height && bitmap.Size.Width <= 250;
    }
}