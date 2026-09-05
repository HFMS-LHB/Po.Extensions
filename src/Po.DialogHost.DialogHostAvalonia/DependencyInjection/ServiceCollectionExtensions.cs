using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Po.DialogHost.Core;
using Po.DialogHost.Core.Interfaces;

namespace Po.DialogHost.DialogHostAvalonia.DependencyInjection;

/// <summary>
/// DialogHost.Avalonia 适配器的依赖注入扩展。
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册 Po.DialogHost 服务，使用 DialogHost.Avalonia 作为弹窗宿主。
    /// </summary>
    public static IServiceCollection AddPoDialogHostWithDialogHostAvalonia(this IServiceCollection services)
    {
        services.TryAddSingleton<IDialogHostAdapter, DialogHostAvaloniaAdapter>();
        services.TryAddSingleton<IPoDialogService, PoDialogService>();

        return services;
    }
}
