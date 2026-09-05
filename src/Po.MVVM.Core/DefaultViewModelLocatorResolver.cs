using Po.MVVM.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.MVVM.Core;

public class DefaultViewModelLocatorResolver : IViewModelLocatorResolver
{
    private readonly IServiceProvider _serviceProvider;

    private readonly IViewModelTypeResolver _typeResolver;


    public DefaultViewModelLocatorResolver(
        IServiceProvider serviceProvider,
        IViewModelTypeResolver typeResolver)
    {
        _serviceProvider = serviceProvider;
        _typeResolver = typeResolver;
    }


    public object? Resolve(Type viewType)
    {
        var vmType =
            _typeResolver.Resolve(viewType);


        if (vmType == null)
            return null;


        return _serviceProvider
            .GetService(vmType);
    }
}
