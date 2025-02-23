using System.Reflection;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Core;
namespace Flash.Configuration.Common.Handlers.AssemblyParser;

internal sealed class PropertyProcessor(ISectionProcessor sectionProcessor) : IPropertyProcessor
{
    public void ProcessProperty(object instance, PropertyInfo property,
        Dictionary<string, object> configValues, string environment)
    {
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(property);
        ArgumentNullException.ThrowIfNull(configValues);
        ArgumentNullException.ThrowIfNull(environment);

        var attribute = property.GetCustomAttribute<FlashPropertyAttribute>();
        if (attribute is null) return;

        var attrName = attribute.DisplayName ?? property.Name;
        var value = property.GetValue(instance);

        var attValue = property.GetCustomAttributes<FlashValueAttribute>()
            .FirstOrDefault(a => string.Equals(a.Environment, environment, StringComparison.OrdinalIgnoreCase) ||
                                 (string.IsNullOrEmpty(a.Environment) && string.Equals(environment, "None",
                                     StringComparison.OrdinalIgnoreCase)));

        var isValueIgnored = property.GetCustomAttributes<FlashValueIgnoreAttribute>().Any(
            a => string.Equals(a.Environment, environment, StringComparison.OrdinalIgnoreCase) ||
                 (string.IsNullOrEmpty(a.Environment) &&
                  string.Equals(environment, "None", StringComparison.OrdinalIgnoreCase)));

        if (isValueIgnored) return;
        if (attValue is not null)
        {
            ProcessDefaultProperty(property, configValues, attValue, attrName);
            return;
        }

        ProcessExtendedProperty(property, configValues, environment, value, attribute, attrName);
    }

    internal void ProcessDefaultProperty(PropertyInfo property,
        Dictionary<string, object> configValues,
        FlashValueAttribute attValue,
        string attrName)
    {
        if (property.PropertyType.IsEnum && !property.IsDefined(typeof(FlashEnumAttribute)) &&
            property.PropertyType.IsDefined(typeof(FlashEnumStringAttribute)))
        {
            if (Enum.GetName(property.PropertyType, attValue.DefaultValue!) is { } enumName)
                configValues[attrName] = enumName;
            return;
        }

        configValues[attrName] = attValue.DefaultValue!;
    }

    internal void ProcessExtendedProperty(PropertyInfo property,
        Dictionary<string, object> configValues,
        string environment, 
        object? value,
        FlashPropertyAttribute attribute,
        string attrName)
    {
        if (property.PropertyType.IsEnum &&
            !property.IsDefined(typeof(FlashEnumAttribute)) &&
            property.PropertyType.IsDefined(typeof(FlashEnumStringAttribute)))
            value = Enum.GetName(property.PropertyType, value!);

        switch (value)
        {
            case null:
                return;

            case IEnumerable<object> collection when attribute.IsComplex:
            {
                var processedCollection = collection
                    .Select(item => sectionProcessor.ProcessComplexValue(item, environment)).ToList();
                configValues[attrName] = processedCollection;
                break;
            }
            case IEnumerable<object> collection:
                configValues[attrName] = collection.ToList();
                break;

            default:
                configValues[attrName] =
                    attribute.IsComplex ? sectionProcessor.ProcessComplexValue(value, environment) : value;
                break;
        }
    }
}