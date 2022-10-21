using App3.Activation;
using App3.Contracts.Services;
using App3.Core.Contracts.Services;
using App3.Core.Services;
using App3.Helpers;
using App3.Models;
using App3.Services;
using App3.ViewModels;
using App3.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;

namespace App3;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    private readonly IActivationService _activationService;
    private readonly ILogger<App> _logger;

    public App(IServiceProvider serviceProvider, IActivationService activationService, ILogger<App> logger)
    {
        ServiceProvider = serviceProvider;
        _activationService = activationService;
        _logger = logger;
        
        InitializeComponent();
        UnhandledException += App_UnhandledException;
    }

    public IServiceProvider ServiceProvider { get; }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.ServiceProvider.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        _logger.LogCritical("Unhandled exception occured {e}", e);
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await _activationService.ActivateAsync(args);
    }
}
