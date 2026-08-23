using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core;

/// <summary>
/// 应用运行环境信息。
/// </summary>
public sealed class AppEnvironment
{
    /// <summary>
    /// 环境名称，例如 Development、Production、Staging。
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 是否为开发环境。
    /// </summary>
    public bool IsDevelopment =>
        Name.Equals("Development", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// 是否为生产环境。
    /// </summary>
    public bool IsProduction =>
        Name.Equals("Production", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// 是否为测试环境。
    /// </summary>
    public bool IsStaging =>
        Name.Equals("Staging", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// 创建运行环境信息。
    /// </summary>
    /// <param name="name">环境名称。</param>
    public AppEnvironment(string name)
    {
        Name = string.IsNullOrWhiteSpace(name)
            ? "Production"
            : name;
    }

    /// <summary>
    /// 从当前运行环境创建 <see cref="AppEnvironment"/>。
    /// </summary>
    public static AppEnvironment FromEnvironment()
    {
        var name = System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        if (string.IsNullOrWhiteSpace(name))
        {
            name = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
#if DEBUG
            name = "Development";
#else
            name = "Production";
#endif
        }

        return new AppEnvironment(name);
    }

    public override string ToString() => Name;

    public bool Is(string environmentName)
    {
        return Name.Equals(environmentName, StringComparison.OrdinalIgnoreCase);
    }
}
