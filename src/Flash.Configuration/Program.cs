using Flash.Configuration.Updater;
using Flash.Configuration.Updater.Abstraction;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddServiceHandlers();
    })
    .Build();

    var appHandler = host.Services.GetRequiredService<IAppHandler>();
    await appHandler.HandleAsync(args);