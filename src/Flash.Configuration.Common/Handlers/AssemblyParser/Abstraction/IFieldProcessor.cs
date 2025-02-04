using System.Reflection;
namespace Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;

public interface IFieldProcessor
{
    Task ProcessFieldAsync(object instance, FieldInfo field, Dictionary<string, object> configValues);
}