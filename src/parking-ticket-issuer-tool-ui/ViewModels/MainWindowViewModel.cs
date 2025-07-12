using System.IO;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ParkingTicketIssuerToolUI.Entities;
using ParkingTicketIssuerToolUI.Messages;
using ParkingTicketIssuerToolUI.Services;

namespace ParkingTicketIssuerToolUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private Brush parkingTicketForeground;

    [ObservableProperty]
    private Brush parkingTicketBackground;

    [ObservableProperty]
    private Brush settingsForeground;

    [ObservableProperty]
    private Brush settingsBackground;

    [ObservableProperty]
    private bool featuresActive;

    [ObservableProperty]
    private ViewModelBase? currentMainViewModel;
    private readonly ILocatorService locatorService;
    private readonly SettingsService settingsService;

    public int ButtonWidth => 250;
    public int ButtonHeight => 100;

    public ColorStyles ColorStyles { get; } = new ColorStyles();

    public MainWindowViewModel(ILocatorService locatorService, SettingsService settingsService)
    {
        var settings = settingsService.GetSettings();
        FeaturesActive = ValidateSettings(settings);

        //Load parking ticket tool
        if (!featuresActive)
        {
            CurrentMainViewModel = locatorService.GetService<SettingsViewModel>();
        }
        parkingTicketForeground = ColorStyles.ButtonBackgroundFontColor;
        parkingTicketBackground = ColorStyles.ButtonBackgroundColor;
        settingsForeground = ColorStyles.ButtonBackgroundFontColor;
        settingsBackground = ColorStyles.ButtonBackgroundColor;

        WeakReferenceMessenger.Default.Register<SettingsChangedMessage>(this, (r, m) =>
        {
            var settings = m.Value;
            if (settings != null)
            {
                if (ValidateSettings(settings))
                {
                    FeaturesActive = true;
                }
            }
        });
        this.locatorService = locatorService;
        this.settingsService = settingsService;
    }

    private bool ValidateSettings(ApplicationSettings settings)
    {
        return File.Exists(settings.LogoPath);
    }

    [RelayCommand]
    private void ChangeMainView(string viewName)
    {
        switch (viewName)
        {
            case "ParkingTicketView":
                break;
            case "SettingsView":
                CurrentMainViewModel = locatorService.GetService<SettingsViewModel>();
                break;
        }
    }
}