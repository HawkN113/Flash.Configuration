using System.Reflection;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Core;
namespace Flash.Configuration.Common.Handlers.AssemblyParser;

public sealed class FieldProcessor : IFieldProcessor
{
    public Task ProcessFieldAsync(object instance, FieldInfo field, Dictionary<string, object> configValues)
    {
        var attribute = field.GetCustomAttribute<FlashFieldAttribute>();
        var attrName = attribute!.DisplayName ?? field.Name;
        var value = field.GetValue(instance);
        if (value is null) return Task.CompletedTask;

        configValues[attrName] = value;
        return Task.CompletedTask;
    }
}