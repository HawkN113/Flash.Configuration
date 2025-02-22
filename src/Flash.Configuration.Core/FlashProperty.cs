namespace Flash.Configuration.Core;

/// <summary>
/// Mark a 'Property' for configuration
/// </summary>
/// <param name="displayName">Display name of property in configuration</param>
/// <param name="isComplex">Should be true, in the case, if you use class as property type.</param>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Property,  AllowMultiple = true)]
public sealed class FlashPropertyAttribute(string? displayName = null, bool isComplex = false) : Attribute
{
    /// <summary>
    /// Display name
    /// </summary>
    public string? DisplayName { get; } = displayName;
    /// <summary>
    /// Flag for complex property type
    /// </summary>
    public bool IsComplex { get; } = isComplex;
}