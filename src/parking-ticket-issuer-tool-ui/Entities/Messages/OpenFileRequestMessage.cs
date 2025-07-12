using System.Collections.Generic;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ParkingTicketIssuerToolUI.Entities.Messages;

public class OpenFileRequestMessage : AsyncRequestMessage<List<string>>
{
    public bool AllowMultiple { get; init; } = false;
    public string Title { get; init; } = "Select Files";
    public List<FilePickerFileType> Filters { get; init; } = new List<FilePickerFileType>();
}