using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.SingleInstance;

/// <summary>
/// 单实例服务工厂
/// </summary>
public static class SingleInstanceServiceFactory
{
    public static ISingleInstanceService Create()
    {
        var appInfo = AppInfo.FromEntryAssembly();

        return Create(new SingleInstanceOptions(appInfo.AppName, appInfo.AppName));
    }

    public static ISingleInstanceService Create(AppInfo appInfo)
    {
        ArgumentNullException.ThrowIfNull(appInfo);

        return Create(new SingleInstanceOptions(appInfo.AppName, appInfo.AppName));
    }

    /// <summary>
    /// 创建适合当前平台的单实例服务
    /// </summary>
    /// <param name="options">单实例配置选项</param>
    /// <returns>平台特定的单实例服务实现</returns>
    /// <exception cref="ArgumentNullException">options 为 null</exception>
    /// <exception cref="PlatformNotSupportedException">当平台不受支持时抛出</exception>
    public static ISingleInstanceService Create(SingleInstanceOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (OperatingSystem.IsWindows())
        {
            return new WindowsSingleInstanceService(options);
        }

        if (OperatingSystem.IsLinux())
        {
            return new LinuxSingleInstanceService(options);
        }

        if (OperatingSystem.IsMacOS())
        {
            return new LinuxSingleInstanceService(options);
        }

        throw new PlatformNotSupportedException(
            $"不支持的操作系统: {Environment.OSVersion.Platform}。单实例检测仅在 Windows 和 Linux 上可用。");
    }
}
