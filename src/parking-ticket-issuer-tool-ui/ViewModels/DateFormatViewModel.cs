using CommunityToolkit.Mvvm.ComponentModel;

namespace ParkingTicketIssuerToolUI.ViewModels;

public partial class DateFormatViewModel : ViewModelBase
{
    public readonly DateFormatConfig DateFormatConfig;

    [ObservableProperty]
    private string displayName;

    [ObservableProperty]
    private string format;

    public DateFormatViewModel(DateFormatConfig dateFormatConfig)
    {
        DateFormatConfig = dateFormatConfig;
        displayName = DateFormatConfig.DisplayName;
        if (displayName == DateFormatConfig.DisplayName)
        {
            displayName = DateFormatConfig.FallbackDisplayName;
        }
        format = $"( {DateFormatConfig.Format} )";
    }
}