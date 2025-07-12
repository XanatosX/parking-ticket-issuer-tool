using Avalonia.Media;

namespace ParkingTicketIssuerToolUI.Entities;

public class ColorStyles
{
    public Brush ButtonSelectedColor { get; init; }
    public Brush ButtonBackgroundColor { get; init; }

    public Brush ButtonSelectedFontColor { get; init; }
    public Brush ButtonBackgroundFontColor { get; init; }

    public ColorStyles()
    {
        ButtonSelectedColor = new SolidColorBrush(Colors.DarkBlue);
        ButtonBackgroundColor = new SolidColorBrush(Colors.DarkGray);
        ButtonSelectedFontColor = new SolidColorBrush(Colors.White);
        ButtonBackgroundFontColor = new SolidColorBrush(Colors.White);
    }

}