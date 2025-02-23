using System.Reflection;
namespace Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;

public interface IFieldProcessor
{
    /// <summary>
    /// Process a field information
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="field"></param>
    /// <param name="configValues"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    void ProcessField(object instance, FieldInfo field, Dictionary<string, object> configValues, string environment);
}