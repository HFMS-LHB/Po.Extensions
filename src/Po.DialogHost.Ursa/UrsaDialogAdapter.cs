using Avalonia.Threading;

using Po.DialogHost.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Ursa.Controls;

namespace Po.DialogHost.Ursa;

/// <summary>
/// 针对 Ursa <see cref="OverlayDialog"/> 的 <see cref="IDialogHostAdapter"/> 实现。
/// </summary>
public sealed class UrsaDialogAdapter : IDialogHostAdapter
{
    private readonly Dictionary<string, Stack<UrsaDialogProxy>> _openProxies = new();

    /// <inheritdoc />
    public Task<object?> ShowAsync(
        object content,
        string? hostIdentifier = null,
        CancellationToken cancellationToken = default)
    {
        var id = hostIdentifier ?? "Main";

        return Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var proxy = new UrsaDialogProxy(content);
            TrackProxy(id, proxy);

            if (content is IPoDialogSessionAware aware)
            {
                aware.SetSession(proxy.Session);
            }

            if (content is IPoDialogPolicy policy)
            {
                policy.OnDialogOpened();
            }

            try
            {
                return await OverlayDialog.ShowCustomAsync<object?>(
                    proxy,
                    proxy,
                    id,
                    CreateOptions(content),
                    cancellationToken);
            }
            finally
            {
                UntrackProxy(proxy);
                proxy.NotifyClosed();
            }
        }, DispatcherPriority.Background);
    }

    /// <inheritdoc />
    public void Close(string? hostIdentifier = null, object? parameter = null)
    {
        var id = hostIdentifier ?? "Main";

        Dispatcher.UIThread.Post(() =>
        {
            if (_openProxies.TryGetValue(id, out var stack) && stack.Count > 0)
            {
                stack.Pop().Session.Close(parameter);
            }
        }, DispatcherPriority.Background);
    }

    /// <inheritdoc />
    public bool IsDialogOpen(string? hostIdentifier = null)
    {
        var id = hostIdentifier ?? "Main";
        return _openProxies.TryGetValue(id, out var stack) && stack.Count > 0;
    }

    private static OverlayDialogOptions CreateOptions(object content)
    {
        var options = new OverlayDialogOptions();

        if (content is IPoDialogPolicy policy)
        {
            options.CanLightDismiss = policy.CloseOnClickAway;
            options.IsCloseButtonVisible = policy.CanClose;
        }

        return options;
    }

    private void TrackProxy(string hostId, UrsaDialogProxy proxy)
    {
        if (!_openProxies.TryGetValue(hostId, out var stack))
        {
            stack = new Stack<UrsaDialogProxy>();
            _openProxies[hostId] = stack;
        }

        stack.Push(proxy);
    }

    private void UntrackProxy(UrsaDialogProxy proxy)
    {
        foreach (var (hostId, stack) in _openProxies)
        {
            if (stack.Count == 0)
            {
                continue;
            }

            // Ursa 的 ShowCustomAsync 是模态的，通常按后进先出关闭。
            if (stack.Peek() == proxy)
            {
                stack.Pop();
                return;
            }

            // 异常顺序时重新整理栈。
            var list = new List<UrsaDialogProxy>(stack);
            if (list.Remove(proxy))
            {
                stack.Clear();
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    stack.Push(list[i]);
                }

                return;
            }
        }
    }
}
