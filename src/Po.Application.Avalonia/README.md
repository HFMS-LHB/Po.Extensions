# Po.Application.Avalonia

## Install

```powershell
dotnet add package Po.Application.Avalonia
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
                services.AddPoApplicationAvalonia();
            });
    }
```