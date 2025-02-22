using System.Reflection;
namespace Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;

public interface IPropertyProcessor
{
    /// <summary>
    /// Process a property information
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="property"></param>
    /// <param name="configValues"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    void ProcessProperty(object instance, PropertyInfo property, Dictionary<string, object> configValues, string environment);
}