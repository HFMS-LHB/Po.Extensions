using System.Reflection;

namespace Po.Application.Core;

public sealed record AppInfo
{
    public required string AppName { get; init; }
    public string AssemblyTitle { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string Copyright { get; init; } = string.Empty;
    public string SupportUrl { get; init; } = string.Empty;
    public DateTime BuildTime { get; init; }

    /// <summary>
    /// 根据指定程序集创建 AppInfo。
    /// </summary>
    public static AppInfo FromAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        var version = assembly.GetName().Version;

        return new AppInfo
        {
            AppName = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product
                      ?? assembly.GetName().Name
                      ?? "Unknown App",

            AssemblyTitle = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title
                            ?? string.Empty,

            Version = version is null
                ? string.Empty
                : $"{version.Major}.{version.Minor}.{version.Build}",

            Copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright
                        ?? string.Empty,

            BuildTime = File.Exists(assembly.Location)
                ? File.GetCreationTime(assembly.Location)
                : DateTime.MinValue
        };
    }

    /// <summary>
    /// 根据入口程序集创建 AppInfo。
    /// </summary>
    public static AppInfo FromEntryAssembly()
    {
        return FromAssembly(Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly());
    }
}