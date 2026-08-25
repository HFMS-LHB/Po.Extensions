using DialogHostAvalonia;

using Po.DialogHost.Avalonia.Dialogs;

using System;
using System.Threading.Tasks;

namespace Po.DialogHost.Avalonia.Interfaces;

public interface IPoDialogService
{
    /// <summary>
    /// 从依赖注入容器中获取指定类型的对话框 ViewModel 实例。
    /// </summary>
    /// <typeparam name="TDialog">对话框 ViewModel 类型。</typeparam>
    /// <returns>指定类型的对话框 ViewModel 实例。</returns>
    TDialog GetDialogViewModel<TDialog>() where TDialog : class;

    /// <summary>
    /// 显示指定内容的对话框。
    /// </summary>
    /// <param name="content">对话框内容，可以是 View 或 ViewModel。</param>
    /// <param name="hostIdentifier">DialogHost 标识。</param>
    /// <returns>对话框关闭时返回的结果。</returns>
    Task<object?> ShowAsync(object content, string hostIdentifier = "Main");

    /// <summary>
    /// 显示指定内容的对话框，并处理打开事件。
    /// </summary>
    /// <param name="content">对话框内容，可以是 View 或 ViewModel。</param>
    /// <param name="openedHandler">对话框打开事件处理。</param>
    /// <param name="hostIdentifier">DialogHost 标识。</param>
    /// <returns>对话框关闭时返回的结果。</returns>
    Task<object?> ShowAsync(object content, Action<DialogOpenedEventArgs>? openedHandler, string hostIdentifier = "Main");

    /// <summary>
    /// 显示指定内容的对话框，并处理打开和关闭事件。
    /// </summary>
    /// <param name="content">对话框内容，可以是 View 或 ViewModel。</param>
    /// <param name="openedHandler">对话框打开事件处理。</param>
    /// <param name="closingHandler">对话框关闭事件处理。</param>
    /// <param name="hostIdentifier">DialogHost 标识。</param>
    /// <returns>对话框关闭时返回的结果。</returns>
    Task<object?> ShowAsync(object content, DialogOpenedEventHandler? openedHandler, DialogClosingEventHandler? closingHandler, string hostIdentifier = "Main");

    /// <summary>
    /// 显示指定内容的对话框，并初始化对话框数据。
    /// </summary>
    /// <typeparam name="TData">对话框数据类型。</typeparam>
    /// <param name="content">实现 <see cref="PoDialogBase{TData}"/> 的对话框 ViewModel。</param>
    /// <param name="data">初始化对话框的数据。</param>
    /// <param name="hostIdentifier">DialogHost 标识。</param>
    /// <returns>对话框关闭时返回的结果。</returns>
    Task<object?> ShowAsync<TData>(object content, TData data, string hostIdentifier = "Main");

    /// <summary>
    /// 创建并显示指定类型的对话框。
    /// <para>
    /// 方法会从依赖注入容器获取 ViewModel，并使用传入的数据初始化。
    /// </para>
    /// </summary>
    /// <typeparam name="TDialog">对话框 ViewModel 类型。</typeparam>
    /// <typeparam name="TData">初始化数据类型。</typeparam>
    /// <param name="data">初始化对话框的数据。</param>
    /// <param name="hostIdentifier">DialogHost 标识。</param>
    /// <returns>对话框关闭时返回的结果。</returns>
    Task<object?> ShowAsync<TDialog, TData>(TData data, string hostIdentifier = "Main") where TDialog : PoDialogBase<TData>;

    /// <summary>
    /// 关闭指定的对话框。
    /// </summary>
    /// <param name="hostIdentifier">DialogHost 标识。</param>
    /// <param name="parameter">关闭时返回的参数。</param>
    void Close(string hostIdentifier = "Main", object? parameter = null);

    /// <summary>
    /// 判断指定 DialogHost 是否正在显示对话框。
    /// </summary>
    /// <param name="hostIdentifier">DialogHost 标识。</param>
    /// <returns>如果存在打开的对话框返回 <see langword="true"/>，否则返回 <see langword="false"/>。</returns>
    bool IsDialogOpen(string hostIdentifier = "Main");
}