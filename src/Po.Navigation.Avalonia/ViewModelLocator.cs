using Avalonia;
using Avalonia.Controls;

using Po.MVVM.Core.DependencyInjection;
using Po.MVVM.Core.Interfaces;

namespace Po.Navigation.Avalonia;

public class ViewModelLocator
{
    static ViewModelLocator()
    {
        AutoWireViewModelProperty.Changed.AddClassHandler<AvaloniaObject>(
        (obj, args) =>
        {
            if (Design.IsDesignMode) return;

            if (args.NewValue is true)
            {
                WireViewModel(obj);
            }
        });
    }

    public static readonly AttachedProperty<bool> AutoWireViewModelProperty =
    AvaloniaProperty.RegisterAttached<ViewModelLocator, AvaloniaObject, bool>(
        "AutoWireViewModel",
        defaultValue: false);


    public static void SetAutoWireViewModel(AvaloniaObject obj, bool value)
    {
        obj.SetValue(AutoWireViewModelProperty, value);
    }

    public static bool GetAutoWireViewModel(AvaloniaObject obj)
    {
        return obj.GetValue(AutoWireViewModelProperty);
    }

    private static void WireViewModel(AvaloniaObject obj)
    {
        if (PoContainer.Provider == null) return;

        if (obj is not StyledElement element) return;

        var vm = PoContainer.GetRequiredService<IViewModelLocatorResolver>().Resolve(obj.GetType());

        element.DataContext = vm;
    }
}
