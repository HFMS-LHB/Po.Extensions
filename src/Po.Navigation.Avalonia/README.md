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
services.AddNavigation<LoginView, LoginViewModel>();
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