using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.MVVM.Core.DependencyInjection;

public static class PoContainer
{
    private static IServiceProvider? _provider;


    /// <summary>
    /// 当前应用服务容器
    /// </summary>
    public static IServiceProvider Provider
    {
        get
        {
            return _provider ?? throw new InvalidOperationException("PoContainer 尚未初始化，请先调用 Initialize()");
        }
    }


    /// <summary>
    /// 初始化框架服务容器
    /// </summary>
    public static void Initialize(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        if (_provider != null)
        {
            throw new InvalidOperationException("PoContainer 已经初始化");
        }

        _provider = provider;
    }


    public static T GetRequiredService<T>()
        where T : notnull
    {
        return Provider.GetRequiredService<T>();
    }


    public static object GetRequiredService(Type serviceType)
    {
        return Provider.GetRequiredService(serviceType);
    }


    public static T? GetService<T>()
        where T : class
    {
        return Provider.GetService<T>();
    }
}
