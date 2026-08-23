using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

using Microsoft.Extensions.Options;

using Po.Application.Avalonia.Windows;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Avalonia.Monitors;

/// <summary>
/// 用户活动监听器。
/// </summary>
public sealed class UserActivityMonitor : IUserActivityMonitor, IDisposable
{
    private readonly IMainWindowProvider _windowProvider;
    private readonly UserActivityMonitorOptions _options;

    private Window? _attachedWindow;

    private bool _enabled;

    private bool _disposed;

    public UserActivityMonitor(IMainWindowProvider windowProvider, IOptions<UserActivityMonitorOptions> options)
    {
        _windowProvider = windowProvider;
        _options = options.Value;

        LastActivityTime = DateTime.Now;

        _windowProvider.MainWindowChanged += OnMainWindowChanged;

        // 默认启动
        Start();
    }

    public bool IsRunning => _enabled;

    /// <summary>
    /// 最后一次用户活动时间。
    /// </summary>
    public DateTime LastActivityTime { get; private set; }

    /// <summary>
    /// 当前空闲时间。
    /// </summary>
    public TimeSpan IdleTime => DateTime.Now - LastActivityTime is var idle && idle > TimeSpan.Zero ? idle : TimeSpan.Zero;

    public void Start()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(UserActivityMonitor));

        if (IsRunning)
            return;

        _enabled = true;

        if (_windowProvider.MainWindow is { } window)
        {
            Attach(window);
        }
    }

    public void Stop()
    {
        if (_disposed)
            return;

        _enabled = false;

        if (_attachedWindow is { } window)
        {
            Detach(window);
            _attachedWindow = null;
        }
    }

    /// <summary>
    /// 用户发生活动时触发。
    /// </summary>
    public event Action? ActivityOccurred;

    private void OnMainWindowChanged(Window window)
    {
        if (_disposed || !_enabled)
            return;

        Attach(window);
    }

    private void Attach(Window window)
    {
        if (_disposed)
            return;

        if (ReferenceEquals(_attachedWindow, window))
            return;

        // 如果之前已经监听了其他窗口，先解除。
        if (_attachedWindow != null)
        {
            Detach(_attachedWindow);
        }

        _attachedWindow = window;

        // Pointer
        window.AddHandler(
            InputElement.PointerPressedEvent,
            OnActivity,
            RoutingStrategies.Tunnel);

        window.AddHandler(
            InputElement.PointerReleasedEvent,
            OnActivity,
            RoutingStrategies.Tunnel);

        window.AddHandler(
            InputElement.PointerWheelChangedEvent,
            OnActivity,
            RoutingStrategies.Tunnel);

        if (_options.TreatPointerMoveAsActivity)
        {
            window.AddHandler(
                InputElement.PointerMovedEvent,
                OnActivity,
                RoutingStrategies.Tunnel);
        }

        // Keyboard
        window.AddHandler(
            InputElement.KeyDownEvent,
            OnActivity,
            RoutingStrategies.Tunnel);

        window.AddHandler(
            InputElement.TextInputEvent,
            OnActivity,
            RoutingStrategies.Tunnel);
    }

    private void Detach(Window window)
    {
        window.RemoveHandler(
            InputElement.PointerPressedEvent,
            OnActivity);

        window.RemoveHandler(
            InputElement.PointerReleasedEvent,
            OnActivity);

        window.RemoveHandler(
            InputElement.PointerWheelChangedEvent,
            OnActivity);

        if (_options.TreatPointerMoveAsActivity)
        {
            window.RemoveHandler(
                InputElement.PointerMovedEvent,
                OnActivity);
        }

        window.RemoveHandler(
            InputElement.KeyDownEvent,
            OnActivity);

        window.RemoveHandler(
            InputElement.TextInputEvent,
            OnActivity);
    }

    private void OnActivity(object? sender, RoutedEventArgs e)
    {
        if (_disposed || !IsRunning)
            return;

        LastActivityTime = DateTime.Now;

        ActivityOccurred?.Invoke();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        Stop();

        _windowProvider.MainWindowChanged -= OnMainWindowChanged;

        ActivityOccurred = null;

        _disposed = true;
    }
}
