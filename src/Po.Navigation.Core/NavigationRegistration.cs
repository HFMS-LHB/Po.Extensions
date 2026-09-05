using System;
using System.Collections.Generic;
using System.Text;

namespace Po.Navigation.Core;

public class NavigationRegistration
{
    public required string Key { get; init; }

    public required Type ViewType { get; init; }

    public required Type ViewModelType { get; init; }
}
