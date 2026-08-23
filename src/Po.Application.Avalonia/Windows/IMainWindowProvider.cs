using Avalonia.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Avalonia.Windows;

public interface IMainWindowProvider
{
    Window? MainWindow { get; }

    event Action<Window>? MainWindowChanged;

    /// <summary>
    /// 设置主窗口。
    /// </summary>
    /// <param name="window">主窗口。</param>
    void SetMainWindow(Window window);
}
