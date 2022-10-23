# hosting-winui3

## Introduction
Default WinUI3 project templates are not suitable for clean usage with Generic Host. This library delivers IHostedService implementation as a wrapper around WinUI3 UI layer.
- Integrates WinUI3 layer with Generic Host through ```IHostedService``` implementation
- Enables Dependency Injection into App.cs

## How to use?
Generic Host is usually instantiated at the entry point to the application, i.e. ```Main()``` function.

Default WinUI3 project templates do not come with ```Main()``` function exposed by default, but we can override this easily by making few minor changes.

Step 1: Disable auto-generated Program.cs by defining ```DISABLE_XAML_GENERATED_MAIN``` constant in the project file

```XML
  <PropertyGroup>
    <DefineConstants>DISABLE_XAML_GENERATED_MAIN</DefineConstants>
  </PropertyGroup>
```
Step 2: Add a new Program.cs class

``` C#
public static class Program
{
    static void Main(string[] args) { }
}
```
Step 3: Instantiate IHost and configure the pipeline. Add WinUI3 hosting by invoking ```.UseWinUI3<App>()``` on the IHostBuilder instance.

``` C#
using EMT.Extensions.Hosting.WinUI3; 
```

``` C#
public static class Program
{
    static void Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder()
                             .UseContentRoot(AppContext.BaseDirectory)
                             .UseWinUI3<App>()
                             .ConfigureServices((context, services) =>
                             {
                                 services.AddSingleton<MainWindow>();
                             })
                             .Build();
        host.Run();
    }
}
```

## App class implementation details

Invoking ```UseWinUI3<App>()``` function registers ```IHostedService``` implementation and  ```App``` class with the DI container. When the hosting starts the App will be instantiated and launched.

Below is a sample of ```App``` class taking ```IServiceProvider``` as a dependency then using it to pull ```MainWindow``` instance.

``` C#
    public partial class App : Application
    {
        private Window mainWindow;
        private readonly IServiceProvider provider;
        public App(IServiceProvider provider)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));

            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            mainWindow = provider.GetRequiredService<MainWindow>();
            mainWindow.Activate();
        }
    }
```

## Samples
Sample app can be found under Samples directory.