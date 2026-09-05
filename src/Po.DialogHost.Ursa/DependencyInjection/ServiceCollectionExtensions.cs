using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Po.DialogHost.Core;
using Po.DialogHost.Core.Interfaces;

namespace Po.DialogHost.Ursa.DependencyInjection;

/// <summary>
/// Ursa 适配器的依赖注入扩展。
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册 Po.DialogHost 服务，使用 Ursa OverlayDialog 作为弹窗宿主。
    /// </summary>
    public static IServiceCollection AddPoDialogHostWithUrsa(this IServiceCollection services)
    {
        services.TryAddSingleton<IDialogHostAdapter, UrsaDialogAdapter>();
        services.TryAddSingleton<IPoDialogService, PoDialogService>();

        return services;
    }
}
