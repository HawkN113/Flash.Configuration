using System.Reflection;
namespace Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;

public interface IPropertyProcessor
{
    Task ProcessPropertyAsync(object instance, PropertyInfo property, Dictionary<string, object> configValues);
}