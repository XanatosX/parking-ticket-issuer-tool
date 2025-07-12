using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Messaging;
using ParkingTicketIssuerToolUI.Entities.Messages;

namespace ParkingTicketIssuerToolUI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<OpenFileRequestMessage>(this, async (r, m) =>
        {

            if (m.HasReceivedResponse)
            {
                return;
            }
            Task<List<string>> task = Task.Run(async () =>
            {
                var topLevel = GetTopLevel(this);
                if (topLevel == null || topLevel.StorageProvider == null)
                {
                    return new List<string>();
                }
                var files = await topLevel.StorageProvider.OpenFilePickerAsync(new()
                {
                    AllowMultiple = m.AllowMultiple,
                    Title = m.Title,
                    FileTypeFilter = m.Filters
                });

                return files.Select(file => file.Path.AbsolutePath).ToList();

            });


            m.Reply(task);
        });

        WeakReferenceMessenger.Default.Register<SaveFileRequestMessage>(this, async (r, m) =>
        {
            if (m.HasReceivedResponse)
            {
                return;
            }
            Task<string> task = Task.Run(async () =>
            {
                var topLevel = GetTopLevel(this);
                if (topLevel == null || topLevel.StorageProvider == null)
                {
                    return string.Empty;
                }
                var file = await topLevel.StorageProvider.SaveFilePickerAsync(new()
                {
                    Title = m.Title,
                    SuggestedFileName = m.DefaultFileName,
                    FileTypeChoices = m.Filters
                });

                return file?.Path.AbsolutePath ?? string.Empty;
            });

            m.Reply(task);
        });
    }


    
}