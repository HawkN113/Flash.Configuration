using System.Reflection;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Core;
namespace Flash.Configuration.Common.Handlers.AssemblyParser;

public sealed class AssemblyProcessor(
    IPropertyProcessor propertyProcessor, 
    IFieldProcessor fieldProcessor) : IAssemblyProcessor
{
    private readonly Dictionary<string, Dictionary<string, object>> _configData = new();

    public async Task<Dictionary<string, Dictionary<string, object>>> ParseAssemblyAsync(string assemblyPath)
    {
        if (!File.Exists(assemblyPath))
        {
            return _configData;
        }

        var assembly = Assembly.LoadFile(assemblyPath);
        var types = assembly.GetTypes()
            .Where(t => t.GetCustomAttributes<FlashConfigAttribute>().Any() &&
                        !t.IsDefined(typeof(FlashDisableAttribute))).AsEnumerable();

        foreach (var type in types)
        {
            await ProcessTypeAsync(type);
        }

        return _configData;
    }

    private async Task ProcessTypeAsync(Type type)
    {
        foreach (var configAttribute in type.GetCustomAttributes<FlashConfigAttribute>())
        {
            var environment = configAttribute.Environment ?? "None";
            var configKey = configAttribute.DisplayName ?? type.Name;

            var environmentConfig = GetOrCreateEnvironmentConfig(environment.ToLower());

            if (!TryCreateInstance(type, out var instance)) return;

            var configValues = new Dictionary<string, object>();

            var fields = type.GetFields().Where(f =>
                    f.IsDefined(typeof(FlashFieldAttribute)) &&
                    !f.IsDefined(typeof(FlashDisableAttribute)))
                .AsEnumerable();
            var properties = type.GetProperties().Where(p =>
                    p.IsDefined(typeof(FlashPropertyAttribute)) &&
                    !p.IsDefined(typeof(FlashDisableAttribute)))
                .AsEnumerable();
            
            foreach (var field in fields)
            {
                await fieldProcessor.ProcessFieldAsync(instance, field, configValues);
            }
            foreach (var property in properties)
            {
                await propertyProcessor.ProcessPropertyAsync(instance, property, configValues);
            }

            environmentConfig[configKey] =
                DeepMerge(environmentConfig.GetValueOrDefault(configKey) as Dictionary<string, object>, configValues);
        }
    }

    private Dictionary<string, object> DeepMerge(Dictionary<string, object>? source,
        Dictionary<string, object> destination)
    {
        if (source is null) return new Dictionary<string, object>(destination);

        return destination.Aggregate(new Dictionary<string, object>(source), (result, kvp) =>
        {
            if (result.TryGetValue(kvp.Key, out var existingValue) &&
                existingValue is Dictionary<string, object> existingDict &&
                kvp.Value is Dictionary<string, object> newDict)
                result[kvp.Key] = DeepMerge(existingDict, newDict);
            else
                result[kvp.Key] = kvp.Value;

            return result;
        });
    }

    private Dictionary<string, object> GetOrCreateEnvironmentConfig(string environment)
    {
        if (_configData.TryGetValue(environment, out var environmentConfig)) return environmentConfig;
        environmentConfig = new Dictionary<string, object>();
        _configData[environment] = environmentConfig;
        return environmentConfig;
    }

    private static bool TryCreateInstance(Type type, out object instance)
    {
        try
        {
            instance = Activator.CreateInstance(type) ??
                       throw new InvalidOperationException($"Cannot create instance of {type.Name}");
            return true;
        }
        catch
        {
            instance = null!;
            return false;
        }
    }
}