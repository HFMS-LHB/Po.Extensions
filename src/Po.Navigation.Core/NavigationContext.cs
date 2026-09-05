using System;
using System.Collections.Generic;
using System.Text;

namespace Po.Navigation.Core;

public class NavigationContext
{
    public string RegionName { get; }

    public string Uri { get; }

    public NavigationParameters Parameters { get; }


    public NavigationContext(
        string regionName,
        string uri,
        NavigationParameters parameters)
    {
        RegionName = regionName;
        Uri = uri;
        Parameters = parameters;
    }
}
