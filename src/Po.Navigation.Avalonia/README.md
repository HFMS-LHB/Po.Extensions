# Po.Navigation.Avalonia

Avalonia region navigation.


## Install

```powershell
dotnet add package Po.Navigation.Avalonia
```

## Setup

```csharp
services.AddPoNavigation();
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
        services.AddPoNavigation();

        services.AddNavigation<LoginView, LoginViewModel>();
    }
```

## Use

** axaml **

```csharp
    <ContentControl po:RegionManager.RegionName="MainRegion"/>
```

** ViewModel **

```csharp
    public partial class LoginViewModel : RegionViewModelBase
    {
        private readonly IRegionManager _regionManager;
        public LoginViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private void Submit()
        {
            _regionManager.RequestNavigate("MainRegion", "Dashboard");
        }
    }
```

## Default ViewModel lookup rule 

```csharp

    private static IEnumerable<string> GetViewModelNames(string viewName)
    {
        // Main -> MainViewModel
        yield return $"{viewName}ViewModel";

        // MainView -> MainViewModel
        if (viewName.EndsWith("View"))
        {
            yield return
                $"{viewName[..^4]}ViewModel";
        }

        // MainWindow -> MainViewModel
        if (viewName.EndsWith("Window"))
        {
            yield return
                $"{viewName[..^6]}ViewModel";
        }

        // MainPage -> MainViewModel
        if (viewName.EndsWith("Page"))
        {
            yield return
                $"{viewName[..^4]}ViewModel";
        }
    }

```