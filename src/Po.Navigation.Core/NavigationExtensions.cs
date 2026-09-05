using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.Navigation.Core;

public static class NavigationExtensions
{
    public static IServiceCollection AddNavigation<TView, TViewModel>(this IServiceCollection services, string? key = null)
        where TView : class where TViewModel : class
    {
        services.AddTransient<TView>();
        services.AddTransient<TViewModel>();

        key ??= typeof(TView).Name;
        var obj = new NavigationRegistration
        {
            Key = key,
            ViewType = typeof(TView),
            ViewModelType = typeof(TViewModel)
        };
        services.AddSingleton(obj);

        return services;
    }
}
