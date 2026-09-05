using System;
using System.Collections.Generic;
using System.Text;

namespace Po.MVVM.Core.Interfaces;

public interface IViewModelLocatorResolver
{
    object? Resolve(Type viewType);
}
