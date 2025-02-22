using Flash.Configuration.Updater;
using Flash.Configuration.Updater.Abstraction;
using Microsoft.Extensions.DependencyInjection;
namespace Flash.Configuration.WinForms;

static class Program
{
    [STAThread]
    static async Task Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();
        services.AddServiceHandlers();

        await using var serviceProvider = services.BuildServiceProvider();
        var appHandler = serviceProvider.GetRequiredService<IAppHandler>();
        await appHandler.HandleAsync(args);
    }
}