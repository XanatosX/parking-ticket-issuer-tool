using System;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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
}