using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Avalonia.Windows;

public class MainWindowProvider : IMainWindowProvider
{
    public event Action<Window>? MainWindowChanged;

    private Window? _mainWindow;

    public Window? MainWindow =>
        _mainWindow ?? GetAvaloniaMainWindow();

    public void SetMainWindow(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        if (ReferenceEquals(_mainWindow, window))
            return;

        _mainWindow = window;

        MainWindowChanged?.Invoke(window);
    }

    private static Window? GetAvaloniaMainWindow()
    {
        if (global::Avalonia.Application.Current?.ApplicationLifetime
            is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }

        return null;
    }
}
