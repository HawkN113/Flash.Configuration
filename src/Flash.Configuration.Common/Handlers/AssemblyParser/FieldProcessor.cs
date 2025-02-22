using System.Reflection;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Core;
namespace Flash.Configuration.Common.Handlers.AssemblyParser;

internal sealed class FieldProcessor : IFieldProcessor
{
    public void ProcessField(object instance, FieldInfo field, Dictionary<string, object> configValues,
        string environment)
    {
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(field);
        ArgumentNullException.ThrowIfNull(configValues);
        ArgumentNullException.ThrowIfNull(environment);

        var attribute = field.GetCustomAttribute<FlashFieldAttribute>();
        if (attribute is null) return;

        var attrName = attribute.DisplayName ?? field.Name;
        var value = field.GetValue(instance);

        var attValue = field.GetCustomAttributes<FlashValueAttribute>()
            .FirstOrDefault(a => string.Equals(a.Environment, environment, StringComparison.OrdinalIgnoreCase) ||
                                 (string.IsNullOrEmpty(a.Environment) && string.Equals(environment, "None",
                                     StringComparison.OrdinalIgnoreCase)));
        var isValueIgnored = field.GetCustomAttributes<FlashValueIgnoreAttribute>().Any(
            a => string.Equals(a.Environment, environment, StringComparison.OrdinalIgnoreCase) ||
                 (string.IsNullOrEmpty(a.Environment) &&
                  string.Equals(environment, "None", StringComparison.OrdinalIgnoreCase)));

        if (attValue is not null && !isValueIgnored)
            value = attValue.DefaultValue;

        if (value is not null && !isValueIgnored)
            configValues[attrName] = value;
    }
}