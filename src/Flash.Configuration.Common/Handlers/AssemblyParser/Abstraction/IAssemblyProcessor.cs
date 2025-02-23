namespace Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;

public interface IAssemblyProcessor
{
    /// <summary>
    /// Get assembly data using full path to the assembly (.dll)
    /// </summary>
    /// <param name="assemblyPath"></param>
    /// <returns></returns>
    Dictionary<string, Dictionary<string, object>> ParseAssembly(string assemblyPath);
}