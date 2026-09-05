using Avalonia.Controls;

using Po.Navigation.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Po.Navigation.Avalonia;

internal interface IAvaloniaRegionManager : IRegionManager
{
    void RegisterRegion(string regionName, ContentControl region);
}
