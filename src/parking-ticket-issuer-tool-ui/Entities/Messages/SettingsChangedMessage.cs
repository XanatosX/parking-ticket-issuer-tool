using Avalonia;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ParkingTicketIssuerToolUI.Entities;

namespace ParkingTicketIssuerToolUI.Messages;

public class SettingsChangedMessage : ValueChangedMessage<ApplicationSettings>
{
    public SettingsChangedMessage(ApplicationSettings value) : base(value)
    {
    }
}