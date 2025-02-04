using System.Reflection;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Core;
namespace Flash.Configuration.Common.Handlers.AssemblyParser;

public sealed class PropertyProcessor(ISectionProcessor sectionProcessor) : IPropertyProcessor
{
    public async Task ProcessPropertyAsync(object instance, PropertyInfo property,
        Dictionary<string, object> configValues)
    {
        var attribute = property.GetCustomAttribute<FlashPropertyAttribute>();
        var attrName = attribute!.DisplayName ?? property.Name;
        var value = property.GetValue(instance);
        if (value is null) return;

        if (property.PropertyType.IsEnum)
        {
            var enumAttribute = property.GetCustomAttribute<FlashEnumAttribute>();
            var enumStringAttribute = property.PropertyType.GetCustomAttribute<FlashEnumStringAttribute>();
            if (enumStringAttribute is not null && enumAttribute is null)
            {
                value = Enum.GetName(property.PropertyType, value)!;
            }
        }

        configValues[attrName] =
            attribute.IsComplex ? await sectionProcessor.ProcessComplexValueAsync(value) : value;
    }
}