using System.Reflection;
using System.Runtime.Loader;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Core;
namespace Flash.Configuration.Common.Handlers.AssemblyParser;

internal sealed class AssemblyProcessor(
    IPropertyProcessor propertyProcessor, 
    IFieldProcessor fieldProcessor) : IAssemblyProcessor
{
    private readonly Dictionary<string, Dictionary<string, object>> _configData = new();

    public Dictionary<string, Dictionary<string, object>> ParseAssembly(string assemblyPath)
    {
        if (string.IsNullOrWhiteSpace(assemblyPath))
            throw new InvalidOperationException("Assembly path cannot be null or empty.");
        if (!File.Exists(assemblyPath))
            return _configData;
        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
        var types = assembly.GetTypes()
            .Where(t => t.GetCustomAttributes<FlashConfigAttribute>().Any() &&
                        !t.IsDefined(typeof(FlashIgnoreAttribute)))
            .OrderBy(t => t.GetCustomAttributes<FlashOrderAttribute>()
                .Select(a => a.Order)
                .DefaultIfEmpty(int.MaxValue)
                .Max())
            .AsEnumerable();

        foreach (var type in types)
            ProcessType(type);

        return _configData;
    }

    private void ProcessType(Type type)
    {
        foreach (var configAttribute in type.GetCustomAttributes<FlashConfigAttribute>())
        {
            var environment = configAttribute.Environment ?? "None";
            var configKey = configAttribute.DisplayName ?? type.Name;

            var environmentConfig = GetOrCreateEnvironmentConfig(environment.ToLower());

            if (!TryCreateInstance(type, out var instance)) return;

            var configValues = new Dictionary<string, object>();

            var fields = GetAllFields(type);
            var properties = GetAllProperties(type);

            foreach (var field in fields)
                fieldProcessor.ProcessField(instance, field, configValues, environment);
            foreach (var property in properties)
                propertyProcessor.ProcessProperty(instance, property, configValues, environment);

            environmentConfig[configKey] =
                DeepMerge(environmentConfig.GetValueOrDefault(configKey) as Dictionary<string, object>, configValues);
        }
    }

    private static IEnumerable<PropertyInfo> GetAllProperties(Type type)
    {
        var properties = type.UnderlyingSystemType
            .GetProperties(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.Default)
            .Where(p =>
                p.IsDefined(typeof(FlashPropertyAttribute)) &&
                !p.IsDefined(typeof(FlashIgnoreAttribute)))
            .AsEnumerable();
        return properties;
    }

    private static IEnumerable<FieldInfo> GetAllFields(Type type)
    {
        var fields = type.UnderlyingSystemType.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly |
                BindingFlags.Instance |
                BindingFlags.Default)
            .Where(f =>
                f.IsDefined(typeof(FlashFieldAttribute)) &&
                !f.IsDefined(typeof(FlashIgnoreAttribute)))
            .AsEnumerable();
        return fields;
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