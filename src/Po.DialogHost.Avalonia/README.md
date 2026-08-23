# Po.DialogHost.Avalonia


## Install

dotnet add package Po.DialogHost.Avalonia

## Setup

```csharp
services.AddPoDialog();
```

## Register

```csharp

    [STAThread]
    public static void Main(string[] args) 
    {
        var host = CreateHostBuilder(args).Build();
        host.Services.InitializePoContainer();
        host.Start();

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddPoMVVM();
        services.AddPoDialog();

        services.AddNavigation<LoginView, LoginViewModel>();
    }
```

## Use

** register vm **

```csharp
services.AddTransient<XXXXDialogViewModel>();
```

** App.xaml **

```csharp
    <Application.DataTemplates>
        <DataTemplate DataType="myApp:XXXXDialogViewModel">
            <myApp:XXXXDialogView />
        </DataTemplate>
    </Application.DataTemplates>
```

** view **

```csharp
<po:PoDialogHost Identifier="Main">
    <!-- view -->
</po:PoDialogHost
```

** ViewModel **

```csharp
public partial class LoginViewModel : ObservableObject
{
    private readonly IPoDialogService _poDialogService;
    public LoginViewModel(IPoDialogService poDialogService)
    {
        _poDialogService = poDialogService;
    }

    private void Submit()
    {
        await _poDialogService.ShowAsync(new ConfirmDialogViewModel());
    }
}
```