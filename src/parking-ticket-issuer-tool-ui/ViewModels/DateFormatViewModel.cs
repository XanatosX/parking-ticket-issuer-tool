using CommunityToolkit.Mvvm.ComponentModel;
using ParkingTicketIssuerTool.Features.Shared;

namespace ParkingTicketIssuerToolUI.ViewModels;

public partial class DateFormatViewModel : ViewModelBase
{
    public readonly DateFormatConfig DateFormatConfig;

    [ObservableProperty]
    private string displayName;

    [ObservableProperty]
    private string format;

    public DateFormatViewModel(DateFormatConfig dateFormatConfig, ITranslationService translationService)
    {
        DateFormatConfig = dateFormatConfig;
        displayName = translationService.Translate(DateFormatConfig.DisplayName);
        if (displayName == DateFormatConfig.DisplayName)
        {
            displayName = DateFormatConfig.FallbackDisplayName;
        }
        format = $"( {DateFormatConfig.Format} )";
    }
}