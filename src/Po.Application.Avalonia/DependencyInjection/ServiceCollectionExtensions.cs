using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Po.Application.Avalonia.FilePickers;
using Po.Application.Avalonia.Monitors;
using Po.Application.Avalonia.Windows;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Avalonia.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPoApplicationAvalonia(this IServiceCollection services, Action<UserActivityMonitorOptions>? configure = null)
    {
        if (configure != null)
        {
            services.Configure(configure);
        }

        services.TryAddSingleton<IMainWindowProvider, MainWindowProvider>();
        services.TryAddSingleton<IFilePickerService, FilePickerService>();
        services.TryAddSingleton<IUserActivityMonitor, UserActivityMonitor>();

        return services;
    }
}
