using Microsoft.Extensions.DependencyInjection;

using Po.MVVM.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.MVVM.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPoMVVM(this IServiceCollection services)
    {
        services.AddSingleton<IViewModelTypeResolver,ConventionViewModelTypeResolver>();
        services.AddSingleton<IViewModelLocatorResolver,DefaultViewModelLocatorResolver>();

        return services;
    }
}
