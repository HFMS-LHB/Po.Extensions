using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.SingleInstance;

/// <summary>
/// 单实例服务配置选项
/// </summary>
public sealed class SingleInstanceOptions
{
    /// <summary>
    /// 互斥锁名称（用于Windows）或锁文件基础名称（用于Linux）
    /// </summary>
    public string MutexName { get; }

    /// <summary>
    /// 窗口标题，用于激活已有实例时查找窗口
    /// </summary>
    public string WindowTitle { get; }

    public Action? OnActivate { get; set; }

    /// <summary>
    /// 创建单实例选项
    /// </summary>
    /// <param name="mutexName">互斥锁名称，建议使用程序集名称</param>
    /// <param name="windowTitle">窗口标题，应与主窗口Title属性一致</param>
    public SingleInstanceOptions(string mutexName, string windowTitle)
    {
        MutexName = mutexName ?? throw new ArgumentNullException(nameof(mutexName));
        WindowTitle = windowTitle ?? throw new ArgumentNullException(nameof(windowTitle));
    }
}
