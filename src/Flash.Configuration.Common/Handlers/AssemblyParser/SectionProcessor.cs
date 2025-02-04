using System.Reflection;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Core;
namespace Flash.Configuration.Common.Handlers.AssemblyParser;

public sealed class SectionProcessor : ISectionProcessor
{
    public async Task<Dictionary<string, object>> ProcessComplexValueAsync(object complexValue)
    {
        var complexData = new Dictionary<string, object>();
        var properties = complexValue.GetType().GetProperties()
            .Where(p =>
                p.IsDefined(typeof(FlashPropertyAttribute)) &&
                !p.IsDefined(typeof(FlashDisableAttribute)));
        foreach (var property in properties)
        {
            var value = property.GetValue(complexValue);
            if (value is null) continue;
            var attrName = property.GetCustomAttribute<FlashPropertyAttribute>()?.DisplayName ?? property.Name;
            complexData[attrName] = value;
        }

        return await Task.FromResult(complexData);
    }
}