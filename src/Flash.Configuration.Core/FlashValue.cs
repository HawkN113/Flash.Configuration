namespace Flash.Configuration.Core;

/// <summary>
/// Add default value for environment
/// </summary>
/// <param name="defaultValue">Default value for the environment</param>
/// <param name="environment">Environment</param>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property,  AllowMultiple = true)]
public sealed class FlashValueAttribute(object? defaultValue = null, string? environment = null) : Attribute
{
    /// <summary>
    /// Default value for the environment
    /// </summary>
    public object? DefaultValue { get; } = defaultValue;
    
    /// <summary>
    /// Environment
    /// </summary>
    public string? Environment { get; } = environment;
}