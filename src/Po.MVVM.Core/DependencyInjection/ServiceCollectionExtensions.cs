using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Po.MVVM.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.MVVM.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPoMVVM(this IServiceCollection services)
    {
        services.TryAddSingleton<IViewModelTypeResolver, ConventionViewModelTypeResolver>();
        services.TryAddSingleton<IViewModelLocatorResolver, DefaultViewModelLocatorResolver>();

        return services;
    }
}
