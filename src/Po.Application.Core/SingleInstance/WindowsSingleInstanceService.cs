using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.SingleInstance;

/// <summary>
/// Windows 平台单实例服务实现
/// </summary>
internal sealed class WindowsSingleInstanceService : ISingleInstanceService
{
    private readonly string _fullMutexName;
    private readonly string _windowTitle;
    private Mutex? _mutex;
    private bool _ownsMutex;

    public WindowsSingleInstanceService(SingleInstanceOptions options)
    {
        var userSid = GetCurrentUserSid();
        _fullMutexName = $"Global\\{options.MutexName}-SingleInstance-{userSid}";
        _windowTitle = options.WindowTitle;
    }

    public bool TryAcquireOwnership()
    {
        try
        {
            // 尝试打开已存在的互斥锁
            try
            {
                _mutex = Mutex.OpenExisting(_fullMutexName);
                CleanupMutex();
                return false;
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                // 互斥锁不存在，继续创建
            }

            // 创建新的互斥锁
            _mutex = new Mutex(initiallyOwned: true, name: _fullMutexName, out var createdNew);

            if (!createdNew)
            {
                CleanupMutex();
                return false;
            }

            _ownsMutex = true;
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            CleanupMutex();
            return false;
        }
        catch (Exception)
        {
            CleanupMutex();
            return true;
        }
    }

    public void ReleaseOwnership()
    {
        if (_ownsMutex && _mutex != null)
        {
            try
            {
                _mutex.ReleaseMutex();
            }
            catch (ApplicationException)
            {
                // 忽略非所有者释放异常
            }
            _ownsMutex = false;
        }
    }

    public void ActivateExistingInstance()
    {
        try
        {
            var hWnd = FindWindow(lpClassName: null, lpWindowName: _windowTitle);

            if (hWnd == IntPtr.Zero)
            {
                hWnd = FindWindow(lpClassName: "AvaloniaWindow", lpWindowName: null);
            }

            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, SW_RESTORE);
                SetForegroundWindow(hWnd);
                FlashWindow(hWnd, bInvert: true);
            }
        }
        catch (Exception)
        {
            // 激活失败不影响主要功能
        }
    }

    public void Dispose()
    {
        ReleaseOwnership();
        _mutex?.Dispose();
        _mutex = null;
    }

    private void CleanupMutex()
    {
        _mutex?.Dispose();
        _mutex = null;
        _ownsMutex = false;
    }

    private static string GetCurrentUserSid()
    {
        try
        {
            using var identity = WindowsIdentity.GetCurrent();
            return identity.User?.Value ?? "default";
        }
        catch
        {
            return Environment.UserName;
        }
    }

    #region Win32 API

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

    private const int SW_RESTORE = 9;

    #endregion
}
