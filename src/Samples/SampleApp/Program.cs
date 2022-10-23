using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using EMT.Extensions.Hosting.WinUI3;

namespace SampleApp
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
                                     // Views and ViewModels
                                     services.AddSingleton<MainWindow>();
                                 })
                                 .Build();
            host.Run();
        }
    }
}