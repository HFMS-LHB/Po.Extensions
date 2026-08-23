using System;
using System.Threading.Tasks;

using DialogHostAvalonia;

namespace Po.DialogHost.Avalonia.Interfaces;

public interface IPoDialogService
{
    Task<object?> ShowAsync(object content, string hostIdentifier = "Main");

    Task<object?> ShowAsync(object content, Action<DialogOpenedEventArgs>? openedHandler, string hostIdentifier = "Main");

    void Close(string hostIdentifier = "Main", object? parameter = null);

    bool IsDialogOpen(string hostIdentifier = "Main");
}