namespace Po.DialogHost.Core.Interfaces;

/// <summary>
/// 表示一个已打开对话框的会话，可用于从 ViewModel 内部关闭对话框并传递结果。
/// </summary>
public interface IPoDialogSession
{
    /// <summary>
    /// 关闭当前对话框，并返回可选结果。
    /// </summary>
    /// <param name="result">对话框结果。</param>
    void Close(object? result = null);

    /// <summary>
    /// 获取当前会话是否已关闭。
    /// </summary>
    bool IsClosed { get; }
}
