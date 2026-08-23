# Po.DialogHost.Avalonia


## Install

dotnet add package Po.DialogHost.Avalonia

## Setup

```csharp
services.AddPoDialog();
```

## Register

```csharp
services.AddNavigation<LoginView, LoginViewModel>();
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