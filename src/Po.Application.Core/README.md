# Po.Application.Core

## Install

```powershell
dotnet add package Po.Application.Core
```

## Use

```csharp

    public static void Main(string[] args) 
    {
        var host = CreateHostBuilder(args).Build();
        host.Start();

        // ...
    }


    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureServices(services => 
            {
                services.AddPoApplicationCore();
            });
    }
```

## Use SingleInstanceManager

```csharp

using var instance = SingleInstanceManager.Acquire();

if (instance is null)
{
    return;
}

```