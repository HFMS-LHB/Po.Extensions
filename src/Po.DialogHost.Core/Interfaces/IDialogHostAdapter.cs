using System.Threading;
using System.Threading.Tasks;

namespace Po.DialogHost.Core.Interfaces;

/// <summary>
/// 对话框宿主适配器抽象。具体控件库（如 DialogHost.Avalonia、Ursa）通过实现此接口接入框架。
/// </summary>
public interface IDialogHostAdapter
{
    /// <summary>
    /// 显示对话框内容。
    /// </summary>
    /// <param name="content">对话框内容，通常是 ViewModel。</param>
    /// <param name="hostIdentifier">宿主标识。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>对话框关闭时返回的结果。</returns>
    Task<object?> ShowAsync(
        object content,
        string? hostIdentifier = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 关闭指定宿主中的对话框。
    /// </summary>
    /// <param name="hostIdentifier">宿主标识。</param>
    /// <param name="parameter">关闭参数。</param>
    void Close(string? hostIdentifier = null, object? parameter = null);

    /// <summary>
    /// 判断指定宿主是否正在显示对话框。
    /// </summary>
    /// <param name="hostIdentifier">宿主标识。</param>
    /// <returns>如果存在打开的对话框返回 <see langword="true"/>，否则返回 <see langword="false"/>。</returns>
    bool IsDialogOpen(string? hostIdentifier = null);
}
