# Po.DialogHost.Ursa

Ursa `OverlayDialog` adapter for `Po.DialogHost`.

## Usage

1. Add `Ursa` styles and an `OverlayDialogHost` to your window:

```xml
<u:UrsaWindow>
    <u:UrsaWindow.Styles>
        <u:UrsaTheme />
    </u:UrsaWindow.Styles>
    <!-- your content -->
    <u:OverlayDialogHost HostId="Main" />
</u:UrsaWindow>
```

2. Register the adapter in DI:

```csharp
services.AddPoDialogHostWithUrsa();
```

3. Use `IPoDialogService` as usual:

```csharp
var result = await _dialogService.ShowAsync<MyDialogViewModel, MyData>(data);
```

## Customizing OverlayDialogOptions

Implement `IUrsaDialogOptionsProvider` on your ViewModel:

```csharp
public class MyDialogViewModel : TaskDialogBase<MyData>, IUrsaDialogOptionsProvider
{
    public OverlayDialogOptions GetOptions()
    {
        return new OverlayDialogOptions
        {
            Title = "My Title",
            FullScreen = false,
            HorizontalAnchor = HorizontalPosition.Center,
            VerticalAnchor = VerticalPosition.Center,
        };
    }
}
```

The adapter will use these options and also apply `CloseOnClickAway` / `CanClose` from `IPoDialogPolicy`.

## Notes

- `CloseOnClickAway` maps to `OverlayDialogOptions.CanLightDismiss`.
- `CanClose` maps to `OverlayDialogOptions.IsCloseButtonVisible`. Because Ursa does not support cancelling a `RequestClose`, the adapter hides the close button while work is in progress instead of blocking the close.
