using System.Windows;
using Flash.Configuration.Updater;
using Flash.Configuration.Updater.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Flash.Configuration.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            base.OnStartup(e);
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(e.Args);
                    services.AddServiceHandlers();
                })
                .Build();

            var args = host.Services.GetRequiredService<string[]>();
            var appHandler = host.Services.GetRequiredService<IAppHandler>();
            await appHandler.HandleAsync(args);
        }
        finally
        {
            Shutdown();
        }
    }
}