using Flash.Configuration.Common;
using Flash.Configuration.Updater.Abstraction;
using Microsoft.Extensions.DependencyInjection;
namespace Flash.Configuration.Updater;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceHandlers(this IServiceCollection services)
    {
        services.AddProcessors();
        services.AddProviders();
        services.AddScoped<IAppValidator, AppValidator>();
        services.AddScoped<IAppHandler, AppHandler>();
        return services;
    }
}