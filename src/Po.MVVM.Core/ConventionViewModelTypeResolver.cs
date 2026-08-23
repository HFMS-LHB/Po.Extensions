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
        return _cache.GetOrAdd(
            viewType,
            ResolveInternal);
    }


    private static Type? ResolveInternal(Type viewType)
    {
        var fullName = viewType.FullName;

        if (string.IsNullOrEmpty(fullName))
            return null;


        var vmName =
            fullName.Replace(
                ".Views.",
                ".ViewModels.");

        var lastDot =
            vmName.LastIndexOf('.');


        if (lastDot > 0)
        {
            vmName =
                $"{vmName[..lastDot]}." +
                $"{vmName[(lastDot + 1)..]}ViewModel";
        }


        return viewType.Assembly
            .GetType(vmName);
    }
}
