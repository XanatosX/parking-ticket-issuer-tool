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
    private Brush? parkingTicketForeground;

    [ObservableProperty]
    private Brush? parkingTicketBackground;

    [ObservableProperty]
    private Brush? settingsForeground;

    [ObservableProperty]
    private Brush? settingsBackground;

    [ObservableProperty]
    private bool featuresActive;

    [ObservableProperty]
    private ViewModelBase? currentMainViewModel;

    [ObservableProperty]
    private string applicationVersion;

    private readonly ILocatorService locatorService;
    private readonly SettingsService settingsService;

    public int ButtonWidth => 250;
    public int ButtonHeight => 100;

    public ColorStyles ColorStyles { get; } = new ColorStyles();

    public MainWindowViewModel(ILocatorService locatorService, SettingsService settingsService, VersionService versionService)
    {
        this.locatorService = locatorService;
        this.settingsService = settingsService;

        applicationVersion = versionService.GetVersion();

        var settings = settingsService.GetSettings();
        FeaturesActive = ValidateSettings(settings);

        ResetColors();

        ChangeMainView("ParkingTicketView");
        if (!featuresActive)
        {
            ChangeMainView("SettingsView");
        }
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
    }

    private void ResetColors()
    {
        ParkingTicketForeground = ColorStyles.ButtonBackgroundFontColor;
        ParkingTicketBackground = ColorStyles.ButtonBackgroundColor;

        SettingsForeground = ColorStyles.ButtonBackgroundFontColor;
        SettingsBackground = ColorStyles.ButtonBackgroundColor;
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
                ResetColors();
                ParkingTicketBackground = ColorStyles.ButtonSelectedColor;
                ParkingTicketForeground = ColorStyles.ButtonSelectedFontColor;
                CurrentMainViewModel = locatorService.GetService<ParkingTicketViewModel>();
                break;
            case "SettingsView":
                ResetColors();
                SettingsBackground = ColorStyles.ButtonSelectedColor;
                SettingsForeground = ColorStyles.ButtonSelectedFontColor;
                CurrentMainViewModel = locatorService.GetService<SettingsViewModel>();
                break;
        }
    }
}