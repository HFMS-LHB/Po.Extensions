using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Po.Application.Core.Lifecycle;
using Po.Application.Core.Paths;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPoApplicationCore(this IServiceCollection services,
        AppInfo? appInfo = null)
    {
        appInfo ??= AppInfo.FromEntryAssembly();

        services.TryAddSingleton(appInfo);

        services.TryAddSingleton<IPathProvider, PathProvider>();
        services.TryAddSingleton<IApplicationReady, ApplicationReady>();

        return services;
    }
}
