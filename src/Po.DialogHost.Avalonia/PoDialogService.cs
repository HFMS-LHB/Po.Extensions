using System;
using System.Threading.Tasks;
using Avalonia.Threading;

using DialogHostAvalonia;

using Po.DialogHost.Avalonia.Interfaces;

namespace Po.DialogHost.Avalonia;

public class PoDialogService : IPoDialogService
{
    public async Task<object?> ShowAsync(object content, string hostIdentifier = "Main")
    {
        return await Dispatcher.UIThread.InvokeAsync(() => DialogHostAvalonia.DialogHost.Show(content, hostIdentifier), DispatcherPriority.Background);
    }

    public async Task<object?> ShowAsync(object content, Action<DialogOpenedEventArgs>? openedHandler, string hostIdentifier = "Main")
    {
        return await Dispatcher.UIThread.InvokeAsync(() => DialogHostAvalonia.DialogHost.Show(content, hostIdentifier, (s, e) => openedHandler?.Invoke(e)), DispatcherPriority.Background);
    }

    public void Close(string hostIdentifier = "Main", object? parameter = null)
    {
        Dispatcher.UIThread.Post(() =>
        {
            try
            {
                if (DialogHostAvalonia.DialogHost.IsDialogOpen(hostIdentifier))
                {
                    DialogHostAvalonia.DialogHost.Close(hostIdentifier, parameter);
                }
            }
            catch (InvalidOperationException)
            {

            }
        }, DispatcherPriority.Background);
    }

    public bool IsDialogOpen(string hostIdentifier = "Main")
    {
        return Dispatcher.UIThread.Invoke(() => DialogHostAvalonia.DialogHost.IsDialogOpen(hostIdentifier), DispatcherPriority.Background);
    }
}