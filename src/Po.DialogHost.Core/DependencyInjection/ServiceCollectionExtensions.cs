using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Po.DialogHost.Core.Interfaces;

namespace Po.DialogHost.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册 Po.DialogHost 核心服务。调用前必须已注册 <see cref="IDialogHostAdapter"/> 实现。
    /// </summary>
    public static IServiceCollection AddPoDialogHost(this IServiceCollection services)
    {
        services.TryAddSingleton<IPoDialogService, PoDialogService>();

        return services;
    }

    /// <summary>
    /// 注册 Po.DialogHost 核心服务，并指定弹窗宿主适配器。
    /// </summary>
    /// <typeparam name="TAdapter">适配器类型。</typeparam>
    public static IServiceCollection AddPoDialogHost<TAdapter>(this IServiceCollection services)
        where TAdapter : class, IDialogHostAdapter
    {
        services.TryAddSingleton<IDialogHostAdapter, TAdapter>();
        services.TryAddSingleton<IPoDialogService, PoDialogService>();

        return services;
    }
}
