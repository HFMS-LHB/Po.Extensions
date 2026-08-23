using DialogHostAvalonia;

using Po.DialogHost.Avalonia.Dialogs;

using System;
using System.Threading.Tasks;

namespace Po.DialogHost.Avalonia.Interfaces;

public interface IPoDialogService
{
    Task<object?> ShowAsync(object content, string hostIdentifier = "Main");

    Task<object?> ShowAsync(object content, Action<DialogOpenedEventArgs>? openedHandler, string hostIdentifier = "Main");

    Task<object?> ShowAsync(object content, DialogOpenedEventHandler? openedHandler, DialogClosingEventHandler? closingHandler, string hostIdentifier = "Main");

    Task<object?> ShowAsync<TData>(object content, TData data, string hostIdentifier = "Main");

    Task<object?> ShowAsync<TDialog, TData>(TData data, string hostIdentifier = "Main") where TDialog : PoDialogBase<TData>;

    void Close(string hostIdentifier = "Main", object? parameter = null);

    bool IsDialogOpen(string hostIdentifier = "Main");
}