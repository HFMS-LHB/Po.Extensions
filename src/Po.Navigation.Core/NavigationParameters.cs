using System;
using System.Collections.Generic;
using System.Text;

namespace Po.Navigation.Core;

public class NavigationParameters : Dictionary<string, object>
{
    public T? GetValue<T>(string key)
    {
        if (TryGetValue(key, out var value))
        {
            if (value is T result)
            {
                return result;
            }
        }

        return default;
    }


    public bool TryGetValue<T>(string key, out T? value)
    {
        if (TryGetValue(key, out var obj) && obj is T result)
        {
            value = result;
            return true;
        }

        value = default;
        return false;
    }
}
