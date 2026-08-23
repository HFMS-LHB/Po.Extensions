using Microsoft.Extensions.DependencyInjection;

using Po.Navigation.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.Navigation.Avalonia.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPoNavigation(this IServiceCollection services)
    {
        services.AddSingleton<IRegionManager, RegionManager>();
        return services;
    }
}
