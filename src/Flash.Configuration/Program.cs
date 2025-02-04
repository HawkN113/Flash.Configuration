using Flash.Configuration;
using Flash.Configuration.Abstraction;
using Flash.Configuration.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using var host = Host.CreateDefaultBuilder(args)
    // -------------------------- Create the service provider (Dependency Injection) --------------------------
    .ConfigureServices(services =>
    {
        services.AddProcessors();
        services.AddScoped<IAppHandler, AppHandler>();
    })
    .Build();
try
{
    var appHandler = host.Services.GetRequiredService<IAppHandler>();
    await appHandler.HandleAsync(args);
    Environment.Exit(0);
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
    Environment.Exit(-1);
}