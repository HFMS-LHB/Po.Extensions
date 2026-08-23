using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.Paths;

public class PathProvider : IPathProvider
{
    private readonly string _appDataFolder;
    private readonly string _localDataFolder;
    private readonly string _tempFolder;

    public PathProvider(AppInfo appInfo)
    {
        ArgumentNullException.ThrowIfNull(appInfo);

        var appName = appInfo.AppName;

        _appDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            appName);

        _localDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            appName);

        _tempFolder = Path.Combine(
            Path.GetTempPath(),
            appName);
    }

    /// <summary>
    /// 程序安装目录
    /// </summary>
    public string InstallationFolder => AppContext.BaseDirectory;

    public string AppDataFolder => _appDataFolder;

    public string LocalDataFolder => _localDataFolder;

    public string TempFolder => _tempFolder;

    /// <summary>
    /// 获取 AppData 下文件路径，自动创建目录
    /// </summary>
    public string GetAppDataFile(string fileName)
    {
        Directory.CreateDirectory(AppDataFolder);
        return Path.Combine(AppDataFolder, fileName);
    }

    /// <summary>
    /// 获取 LocalAppData 下文件路径，自动创建目录
    /// </summary>
    public string GetLocalDataFile(string fileName)
    {
        Directory.CreateDirectory(LocalDataFolder);
        return Path.Combine(LocalDataFolder, fileName);
    }

    /// <summary>
    /// 获取 Temp 下文件路径，自动创建目录
    /// </summary>
    public string GetTempFile(string fileName)
    {
        Directory.CreateDirectory(TempFolder);
        return Path.Combine(TempFolder, fileName);
    }
}
