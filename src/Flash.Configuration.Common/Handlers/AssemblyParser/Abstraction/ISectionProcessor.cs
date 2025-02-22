namespace Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;

public interface ISectionProcessor
{
    /// <summary>
    /// Process property type (complex type)
    /// </summary>
    /// <param name="complexValue"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    Dictionary<string, object> ProcessComplexValue(object complexValue, string environment);
}