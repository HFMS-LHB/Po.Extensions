# Po.DialogHost.DialogHostAvalonia

DialogHost.Avalonia adapter for `Po.DialogHost`.

## Usage

1. Add DialogHost styles to your `App.axaml`:

```xml
<Application xmlns:dialogHostAvalonia="clr-namespace:DialogHostAvalonia;assembly=DialogHost.Avalonia"
             ...>
    <Application.Styles>
        <FluentTheme />
        <dialogHostAvalonia:DialogHostStyles />
    </Application.Styles>
</Application>
```

2. Register the adapter in DI:

```csharp
services.AddPoDialogHostWithDialogHostAvalonia();
```

3. Place `PoDialogHost` in your window:

```xml
<Window xmlns:poDialog="https://po.mvvm.top/dialoghost"
        ...>
    <poDialog:PoDialogHost Identifier="Main">
        <!-- your content -->
    </poDialog:PoDialogHost>
</Window>
```

4. Use `IPoDialogService` as usual.
