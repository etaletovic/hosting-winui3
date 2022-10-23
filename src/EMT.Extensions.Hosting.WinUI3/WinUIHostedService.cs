using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

namespace EMT.Extensions.Hosting.WinUI3;
internal class WinUIHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WinUIHostedService> _logger;

    static WinUIHostedService()
    {
        PInvokeFunctions.XamlCheckProcessRequirements();
    }

    public WinUIHostedService(IHostApplicationLifetime hostApplicationLifetime,
                              IServiceProvider serviceProvider,
                              ILogger<WinUIHostedService> logger)
    {
        _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {name}...", nameof(WinUIHostedService));
        await Task.CompletedTask;

        var thread = new Thread(RunAppLogic);
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();

        _logger.LogInformation("{name} is running.", nameof(WinUIHostedService));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping {name}...", nameof(WinUIHostedService));

        await Task.CompletedTask;

        _logger.LogInformation("{name} is stopped.", nameof(WinUIHostedService));
    }

    private void RunAppLogic()
    {
        WinRT.ComWrappersSupport.InitializeComWrappers();

        // Application.Start is blocking call
        Application.Start((p) =>
        {
            var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            var context = new DispatcherQueueSynchronizationContext(dispatcherQueue);
            SynchronizationContext.SetSynchronizationContext(context);

            _ = _serviceProvider.GetRequiredService<Application>();
        });

        // Application shutdown initiated at this point
        _hostApplicationLifetime.StopApplication();
    }
}
