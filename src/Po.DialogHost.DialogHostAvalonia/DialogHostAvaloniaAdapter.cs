using Avalonia.Threading;

using Po.DialogHost.Core.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Po.DialogHost.DialogHostAvalonia;

/// <summary>
/// 针对 DialogHost.Avalonia 的 <see cref="IDialogHostAdapter"/> 实现。
/// </summary>
public sealed class DialogHostAvaloniaAdapter : IDialogHostAdapter
{
    public Task<object?> ShowAsync(
        object content,
        string? hostIdentifier = null,
        object? options = null,
        CancellationToken cancellationToken = default)
    {
        var identifier = hostIdentifier ?? "Main";

        // DialogHost.Avalonia 目前不通过 options 接收额外配置，
        // 调用方可忽略此参数或使用未来可能支持的扩展。
        return Dispatcher.UIThread.InvokeAsync(
            () => global::DialogHostAvalonia.DialogHost.Show(content, identifier),
            DispatcherPriority.Background);
    }

    public void Close(string? hostIdentifier = null, object? parameter = null)
    {
        var identifier = hostIdentifier ?? "Main";

        Dispatcher.UIThread.Post(() =>
        {
            try
            {
                if (global::DialogHostAvalonia.DialogHost.IsDialogOpen(identifier))
                {
                    global::DialogHostAvalonia.DialogHost.Close(identifier, parameter);
                }
            }
            catch (InvalidOperationException)
            {
                // 忽略宿主未找到等异常
            }
        }, DispatcherPriority.Background);
    }

    public bool IsDialogOpen(string? hostIdentifier = null)
    {
        var identifier = hostIdentifier ?? "Main";

        return Dispatcher.UIThread.Invoke(
            () => global::DialogHostAvalonia.DialogHost.IsDialogOpen(identifier),
            DispatcherPriority.Background);
    }
}
