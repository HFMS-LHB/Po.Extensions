using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Po.DialogHost.Avalonia.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.DialogHost.Avalonia.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPoDialogHost(this IServiceCollection services)
    {
        services.TryAddSingleton<IPoDialogService, PoDialogService>();

        return services;
    }
}
