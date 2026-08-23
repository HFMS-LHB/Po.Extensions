using Avalonia.Threading;

using DialogHostAvalonia;

using Po.DialogHost.Avalonia.Dialogs;
using Po.DialogHost.Avalonia.Interfaces;
using Po.MVVM.Core.DependencyInjection;

using System;
using System.Threading.Tasks;

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

    public async Task<object?> ShowAsync(object content, DialogOpenedEventHandler? openedHandler, DialogClosingEventHandler? closingHandler, string hostIdentifier = "Main")
    {
        return await Dispatcher.UIThread.InvokeAsync(() => DialogHostAvalonia.DialogHost.Show(content, hostIdentifier, openedHandler, closingHandler), DispatcherPriority.Background);
    }

    public async Task<object?> ShowAsync<TDialog, TData>(TData data, string hostIdentifier = "Main") where TDialog : PoDialogBase<TData>
    {
        var vm = PoContainer.GetRequiredService<TDialog>();

        vm.Initialize(data);

        return await ShowAsync(vm, hostIdentifier);
    }

    public async Task<object?> ShowAsync<TData>(object content, TData data, string hostIdentifier = "Main")
    {
        if (content is PoDialogBase<TData> vm && vm != null)
        {
            vm.Initialize(data);

            return await ShowAsync(content);
        }

        return null;
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