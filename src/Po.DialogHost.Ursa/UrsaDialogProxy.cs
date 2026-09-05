using Avalonia.Controls;

using Irihi.Avalonia.Shared.Contracts;

using Po.DialogHost.Core.Interfaces;

using System;

namespace Po.DialogHost.Ursa;

/// <summary>
/// 桥接 Ursa <see cref="IDialogContext"/> 与 Po 会话体系的代理控件。
/// </summary>
internal sealed class UrsaDialogProxy : ContentControl, IDialogContext
{
    /// <summary>
    /// 初始化 <see cref="UrsaDialogProxy"/> 新实例。
    /// </summary>
    /// <param name="viewModel">对话框 ViewModel。</param>
    public UrsaDialogProxy(object viewModel)
    {
        DataContext = viewModel;
        Content = viewModel;
        Session = new UrsaDialogSession(this);
    }

    /// <summary>
    /// 当前会话。
    /// </summary>
    public UrsaDialogSession Session { get; }

    /// <inheritdoc />
    public event EventHandler<object?>? RequestClose;

    /// <inheritdoc />
    public void Close()
    {
        RequestClose?.Invoke(this, Session.PendingResult);
    }

    /// <summary>
    /// 通知关联的 ViewModel 对话框已关闭。
    /// </summary>
    public void NotifyClosed()
    {
        Session.MarkClosed();

        if (DataContext is IPoDialogPolicy policy)
        {
            policy.OnDialogClosed();
        }
    }
}
