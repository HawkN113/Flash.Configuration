namespace Flash.Configuration.Core;

/// <summary>
/// Mark a 'Field' for configuration
/// </summary>
/// <param name="displayName">Display name of field in configuration</param>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Field,  AllowMultiple = true)]
public sealed class FlashFieldAttribute(string? displayName = null) : Attribute
{
    /// <summary>
    /// Display name
    /// </summary>
    public string? DisplayName { get; } = displayName;
}