using System.Collections.Generic;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ParkingTicketIssuerToolUI.Entities.Messages;

public class SaveFileRequestMessage : AsyncRequestMessage<string>
{
    public string Title { get; init; } = "Save file to ...";

    public string DefaultFileName { get; init; } = string.Empty;
    public List<FilePickerFileType> Filters { get; init; } = new List<FilePickerFileType>();
}