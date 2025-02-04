namespace Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;

public interface IAssemblyProcessor
{
    Task<Dictionary<string, Dictionary<string, object>>> ParseAssemblyAsync(string assemblyPath);
}