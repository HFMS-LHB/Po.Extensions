# Po.MVVM.Core

MVVM utilities for .NET applications.

## Install

```powershell
dotnet add package Po.MVVM.Core
```

## Use

```csharp
     var host = CreateHostBuilder(args).Build();
     host.Services.InitializePoContainer();
```