using Avalonia;
using Avalonia.Controls;

using Microsoft.Extensions.DependencyInjection;

using Po.MVVM.Core.DependencyInjection;
using Po.MVVM.Core.Interfaces;
using Po.Navigation.Core;
using Po.Navigation.Core.Interfaces;

namespace Po.Navigation.Avalonia;

public class RegionManager : IAvaloniaRegionManager
{
    private readonly Dictionary<string, ContentControl> _regions = new();

    private readonly Dictionary<string, NavigationRegistration> _registrations = new();

    private readonly Dictionary<string, Control> _cache = new();

    public RegionManager(IEnumerable<NavigationRegistration> registrations)
    {

        CheckRepeatRegister(registrations);
        _registrations = registrations.ToDictionary(x => x.Key);

        RegionNameProperty.Changed.Subscribe(OnRegionChanged);
    }

    private void CheckRepeatRegister(IEnumerable<NavigationRegistration> registrations)
    {
        var repeat = registrations.GroupBy(x => x.Key)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key);

        if (repeat.Any())
        {
            var repeatKey = string.Join(",", repeat);

            throw new InvalidOperationException(
                $"导航Key重复:{repeatKey}");
        }
    }

    private void OnRegionChanged(AvaloniaPropertyChangedEventArgs<string?> args)
    {
        if (args.Sender is ContentControl control
            && args.NewValue.HasValue
            && args.NewValue.Value is string name)
        {
            RegisterRegion(name, control);
        }
    }

    #region AttachedProperty

    public static readonly AttachedProperty<string?> RegionNameProperty =
        AvaloniaProperty.RegisterAttached<RegionManager, Control, string?>("RegionName");

    public static void SetRegionName(Control element, string? value)
    {
        element.SetValue(RegionNameProperty, value);
    }

    public static string? GetRegionName(Control element)
    {
        return element.GetValue(RegionNameProperty);
    }

    #endregion

    #region Region

    public void RegisterRegion(string regionName, ContentControl control)
    {
        _regions[regionName] = control;
    }

    public void SetRegionContent(string regionName, Control content)
    {
        if (_regions.TryGetValue(regionName, out var region))
        {
            region.Content = content;
        }
        else
        {
            throw new InvalidOperationException($"Region {regionName} 未注册");
        }
    }


    public Control? GetRegionContent(string regionName)
    {
        if (_regions.TryGetValue(regionName, out var region))
        {
            return region.Content as Control;
        }

        return null;
    }


    public void RemoveRegion(string regionName)
    {
        if (_regions.TryGetValue(regionName, out var region))
        {
            region.Content = null;
        }
    }
    #endregion


    #region Navigate

    public void Navigate(string regionName, object view)
    {
        throw new NotImplementedException();
    }

    public async Task RequestNavigate(string regionName, string key, NavigationParameters? parameters = null)
    {
        if (!_regions.TryGetValue(regionName, out var regionControl))
        {
            throw new Exception($"Region不存在:{regionName}");
        }

        if (!_registrations.TryGetValue(key, out var registration))
        {
            throw new Exception($"导航未注册:{key}");
        }

        var context = new NavigationContext(regionName, key, parameters ?? []);

        var oldView = regionControl.Content as Control;

        // 离开确认
        if (oldView?.DataContext is IConfirmNavigationRequest confirm)
        {
            bool canNavigate = false;

            confirm.ConfirmNavigationRequest(context, result =>
            {
                canNavigate = result;
            });

            if (!canNavigate) return;
        }

        // 导航离开
        if (oldView?.DataContext is INavigationAware oldAware)
        {
            oldAware.OnNavigatedFrom(context);

            if (oldView.DataContext is IRegionMemberLifetime lifetime)
            {
                if (lifetime.KeepAlive)
                {
                    _cache[$"{regionName}:{key}"] = oldView;
                }
                else
                {
                    DestroyViewModel(oldView);
                }
            }
        }

        // 检查缓存
        if (_cache.TryGetValue($"{regionName}:{key}", out var cacheView))
        {
            regionControl.Content = cacheView;
            return;
        }

        var vm = PoContainer.GetRequiredService(registration.ViewModelType);
        var view = (Control)PoContainer.GetRequiredService(registration.ViewType)!;
        view.DataContext = vm;

        if (vm is INavigationAware aware)
        {
            if (!aware.IsNavigationTarget(context))
            {
                return;
            }
        }

        regionControl.Content = view;

        if (vm is INavigationAware target)
        {
            target.OnNavigatedTo(context);
        }
    }


    private void DestroyViewModel(object view)
    {
        if (view is Control control && control.DataContext is IDestructible destructible)
        {
            destructible.Destroy();
        }
    }


    #endregion
}
