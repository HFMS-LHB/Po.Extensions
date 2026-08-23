using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.Paths;

public interface IPathProvider
{
    /// <summary>
    /// 程序安装目录
    /// </summary>
    string InstallationFolder { get; }

    /// <summary>
    /// 用户数据根目录
    /// win: Roaming
    /// </summary>
    string AppDataFolder { get; }

    /// <summary>
    /// 本地缓存/日志目录
    /// win: Local
    /// </summary>
    string LocalDataFolder { get; }

    /// <summary>
    /// 临时目录
    /// </summary>
    string TempFolder { get; }

    string GetAppDataFile(string fileName);

    string GetLocalDataFile(string fileName);

    string GetTempFile(string fileName);
}
