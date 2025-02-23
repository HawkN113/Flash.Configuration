using Flash.Configuration.Common.Handlers.AssemblyParser;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Common.Handlers.CommandLineParser;
using Flash.Configuration.Common.Handlers.CommandLineParser.Abstraction;
using Flash.Configuration.Common.Handlers.ConfigParser;
using Flash.Configuration.Common.Handlers.ConfigParser.Abstraction;
using Flash.Configuration.Common.Providers;
using Flash.Configuration.Common.Providers.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Flash.Configuration.Common;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add processors for assembly processing
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddProcessors(this IServiceCollection services)
    {
        services.AddScoped<ICommandProcessor, CommandProcessor>();
        services.AddScoped<IPropertyProcessor, PropertyProcessor>();
        services.AddScoped<IFieldProcessor, FieldProcessor>();
        services.AddScoped<ISectionProcessor, SectionProcessor>();
        services.AddScoped<IAssemblyProcessor, AssemblyProcessor>();
        services.AddScoped<IConfigProcessor, ConfigProcessor>();
        return services;
    }

    /// <summary>
    /// Add providers for assembly procesccing
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IDirectoryProvider, DirectoryProvider>();
        services.AddScoped<IFileProvider, FileProvider>();
        return services;
    }
}