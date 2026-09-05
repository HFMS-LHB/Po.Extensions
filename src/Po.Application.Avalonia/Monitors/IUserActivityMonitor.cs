using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Avalonia.Monitors;

/// <summary>
/// 用户活动监听器。
/// </summary>
public interface IUserActivityMonitor
{
    /// <summary>
    /// 最后一次用户活动时间。
    /// </summary>
    DateTime LastActivityTime { get; }

    /// <summary>
    /// 当前空闲时长。
    /// </summary>
    TimeSpan IdleTime { get; }

    /// <summary>
    /// 是否正在监听。
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// 用户发生操作时触发。
    /// </summary>
    event Action? ActivityOccurred;

    /// <summary>
    /// 开始监听。
    /// </summary>
    void Start();

    /// <summary>
    /// 停止监听。
    /// </summary>
    void Stop();
}
