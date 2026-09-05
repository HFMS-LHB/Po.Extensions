using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Po.Navigation.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.Navigation.Avalonia.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPoNavigation(this IServiceCollection services)
    {
        services.TryAddSingleton<IRegionManager, RegionManager>();
        return services;
    }
}
