namespace Flash.Configuration.Core;

/// <summary>
/// Ignore value for the specific environment
/// </summary>
/// <param name="environment">Environment</param>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property,  AllowMultiple = true)]
public sealed class FlashValueIgnoreAttribute(string? environment = null) : Attribute
{
    /// <summary>
    /// Environment
    /// </summary>
    public string? Environment { get; } = environment;
}