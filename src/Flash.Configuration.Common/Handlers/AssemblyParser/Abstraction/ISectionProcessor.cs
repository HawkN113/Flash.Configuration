namespace Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;

public interface ISectionProcessor
{
    Task<Dictionary<string, object>> ProcessComplexValueAsync(object complexValue);
}