using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace EMT.Extensions.Hosting.WinUI3;
public static class Extensions
{
    public static IHostBuilder UseWinUI3<TApplication>(this IHostBuilder builder)
        where TApplication : Application
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddTransient<Application, TApplication>();
            services.AddHostedService<WinUIHostedService>();
        });
        return builder;
    }
}
