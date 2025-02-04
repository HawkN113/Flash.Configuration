using Flash.Configuration.Common.Handlers.AssemblyParser;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Common.Handlers.CommandLineParser;
using Flash.Configuration.Common.Handlers.CommandLineParser.Abstraction;
using Flash.Configuration.Common.Handlers.ConfigParser;
using Flash.Configuration.Common.Handlers.ConfigParser.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Flash.Configuration.Common;

public static class ServiceCollectionExtensions
{
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
}