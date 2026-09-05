using Po.DialogHost.Core.Interfaces;

namespace Po.DialogHost.Ursa;

/// <summary>
/// Ursa 弹窗会话实现。
/// </summary>
internal sealed class UrsaDialogSession : IPoDialogSession
{
    private readonly UrsaDialogProxy _proxy;

    public UrsaDialogSession(UrsaDialogProxy proxy)
    {
        _proxy = proxy;
    }

    /// <summary>
    /// 等待返回给 Ursa 的结果对象。
    /// </summary>
    public object? PendingResult { get; private set; }

    public bool IsClosed { get; private set; }

    public void Close(object? result = null)
    {
        if (IsClosed)
        {
            return;
        }

        PendingResult = result;
        _proxy.Close();
    }

    internal void MarkClosed()
    {
        IsClosed = true;
    }
}
