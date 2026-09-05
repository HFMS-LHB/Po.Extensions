using Avalonia;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Po.Application.Avalonia.DependencyInjection;
using Po.Application.Core.DependencyInjection;
using Po.Demo.Avalonia.ViewModels;
using Po.Demo.Avalonia.Views;
using Po.DialogHost.DialogHostAvalonia.DependencyInjection;
using Po.DialogHost.Ursa.DependencyInjection;
using Po.MVVM.Core.DependencyInjection;
using Po.Navigation.Avalonia.DependencyInjection;
using Po.Navigation.Core;

using System;

namespace Po.Demo.Avalonia;

class Program
{
    public static IHost Host { get; private set; } = null!;

    // Initialization code. Do not use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things are not initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
#if DEBUG
        TrySetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
#endif

        Host = CreateHostBuilder(args).Build();
        Host.Services.InitializePoContainer();
        Host.Start();

        BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
    }



    // Avalonia configuration, do not remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();


    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddPoMVVM();
                services.AddPoApplicationCore();
                services.AddPoApplicationAvalonia();
                // services.AddPoDialogHostWithDialogHostAvalonia();
                services.AddPoDialogHostWithUrsa();
                services.AddPoNavigation();
            })
            .ConfigureServices(ConfigureServices);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<MainWindow>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<TestDialogView>();
        services.AddTransient<TestDialogViewModel>();

        services.AddNavigation<HomeView, HomeViewModel>();
    }

    private static void TrySetEnvironmentVariable(string name, string value)
    {
        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(name)))
        {
            return;
        }

        Environment.SetEnvironmentVariable(name, value);
    }
}
