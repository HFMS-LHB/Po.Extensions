using Po.DialogHost.Core.Interfaces;

namespace Po.DialogHost.DialogHostAvalonia;

/// <summary>
/// 包装 DialogHost.Avalonia 的 <see cref="global::DialogHostAvalonia.DialogSession"/>，实现 <see cref="IPoDialogSession"/>。
/// </summary>
internal sealed class DialogHostAvaloniaSession : IPoDialogSession
{
    private readonly global::DialogHostAvalonia.DialogSession _session;

    public DialogHostAvaloniaSession(global::DialogHostAvalonia.DialogSession session)
    {
        _session = session;
    }

    public bool IsClosed => _session.IsEnded;

    public void Close(object? result = null)
    {
        if (_session.IsEnded)
        {
            return;
        }

        _session.Close(result);
    }
}
