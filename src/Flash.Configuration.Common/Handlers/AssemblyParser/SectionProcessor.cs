using System.Reflection;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Core;
namespace Flash.Configuration.Common.Handlers.AssemblyParser;

internal sealed class SectionProcessor : ISectionProcessor
{
    public Dictionary<string, object> ProcessComplexValue(object complexValue, string environment)
    {
        ArgumentNullException.ThrowIfNull(complexValue);
        ArgumentNullException.ThrowIfNull(environment);

        var complexData = new Dictionary<string, object>();

        foreach (var property in complexValue.GetType().GetProperties()
                     .Where(p => p.IsDefined(typeof(FlashPropertyAttribute)) &&
                                 !p.IsDefined(typeof(FlashIgnoreAttribute))))
        {
            var value = property.GetValue(complexValue);

            var attValue = property.GetCustomAttributes<FlashValueAttribute>()
                .FirstOrDefault(a => string.Equals(a.Environment, environment, StringComparison.OrdinalIgnoreCase) ||
                                     (string.IsNullOrEmpty(a.Environment) && string.Equals(environment, "None",
                                         StringComparison.OrdinalIgnoreCase)));

            var attrName = property.GetCustomAttribute<FlashPropertyAttribute>()?.DisplayName ?? property.Name;
            if (attValue is not null)
            {
                complexData[attrName] = attValue.DefaultValue!;
                continue;
            }

            if (value is not null)
                complexData[attrName] = value;
        }

        return complexData;
    }
}