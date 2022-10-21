using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using App3.Activation;
using App3.Contracts.Services;
using App3.Core.Contracts.Services;
using App3.Core.Services;
using App3.Models;
using App3.Services;
using App3.ViewModels;
using App3.Views;
using EMT.Extensions.Hosting.WinUI3;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

namespace App3
{
    public static class Program
    {
        static void Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder()
                                 .UseContentRoot(AppContext.BaseDirectory)
                                 .UseWinUI3<App>()
                                 .ConfigureServices((context, services) =>
                                 {
                                     // Default Activation Handler
                                     services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

                                     // Other Activation Handlers

                                     // Services
                                     services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
                                     services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
                                     services.AddTransient<INavigationViewService, NavigationViewService>();

                                     services.AddSingleton<IActivationService, ActivationService>();
                                     services.AddSingleton<IPageService, PageService>();
                                     services.AddSingleton<INavigationService, NavigationService>();

                                     // Core Services
                                     services.AddSingleton<IFileService, FileService>();

                                     // Views and ViewModels

                                     services.AddSingleton<MainWindow>();

                                     services.AddTransient<SettingsViewModel>();
                                     services.AddTransient<SettingsPage>();
                                     services.AddTransient<MainViewModel>();
                                     services.AddTransient<MainPage>();
                                     services.AddTransient<ShellPage>();
                                     services.AddTransient<ShellViewModel>();

                                     // Configuration
                                     services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
                                 })
                                 .Build();
            host.Run();
        }
    }
}