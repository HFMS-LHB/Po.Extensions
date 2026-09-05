using Ursa.Controls;

namespace Po.DialogHost.Ursa;

/// <summary>
/// 由 ViewModel 实现，以自定义 Ursa <see cref="OverlayDialogOptions"/>。
/// </summary>
public interface IUrsaDialogOptionsProvider
{
    /// <summary>
    /// 获取当前对话框的 Ursa 选项。
    /// </summary>
    OverlayDialogOptions GetOptions();
}
