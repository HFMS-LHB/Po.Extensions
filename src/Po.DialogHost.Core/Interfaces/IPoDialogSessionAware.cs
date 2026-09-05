namespace Po.DialogHost.Core.Interfaces;

/// <summary>
/// 标记当前 ViewModel 需要感知对话框会话。
/// </summary>
public interface IPoDialogSessionAware
{
    /// <summary>
    /// 设置当前对话框会话。
    /// </summary>
    /// <param name="session">对话框会话。</param>
    void SetSession(IPoDialogSession session);
}
