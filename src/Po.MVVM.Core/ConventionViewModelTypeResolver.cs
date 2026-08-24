using Po.MVVM.Core.Interfaces;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Po.MVVM.Core;

public class ConventionViewModelTypeResolver
    : IViewModelTypeResolver
{
    private readonly ConcurrentDictionary<Type, Type?> _cache = new();

    public Type? Resolve(Type viewType)
    {
        return _cache.GetOrAdd(viewType, ResolveInternal);
    }


    private static Type? ResolveInternal(Type viewType)
    {
        var fullName = viewType.FullName;

        if (string.IsNullOrEmpty(fullName))
            return null;


        var vmNamespace = fullName.Replace(".Views.", ".ViewModels.");


        var lastDot = vmNamespace.LastIndexOf('.');

        if (lastDot <= 0) return null;

        var namespaceName = vmNamespace[..lastDot];
        var viewName = vmNamespace[(lastDot + 1)..];

        foreach (var vmName in GetViewModelNames(viewName))
        {
            var typeName =
                $"{namespaceName}.{vmName}";

            var vmType =
                viewType.Assembly.GetType(typeName);

            if (vmType != null)
            {
                return vmType;
            }
        }

        return null;
    }

    private static IEnumerable<string> GetViewModelNames(string viewName)
    {
        // 例如 Main -> MainViewModel
        yield return $"{viewName}ViewModel";

        // MainView -> MainViewModel
        if (viewName.EndsWith("View"))
        {
            yield return
                $"{viewName[..^4]}ViewModel";
        }

        // MainWindow -> MainViewModel
        if (viewName.EndsWith("Window"))
        {
            yield return
                $"{viewName[..^6]}ViewModel";
        }

        // MainPage -> MainViewModel
        if (viewName.EndsWith("Page"))
        {
            yield return
                $"{viewName[..^4]}ViewModel";
        }
    }
}
