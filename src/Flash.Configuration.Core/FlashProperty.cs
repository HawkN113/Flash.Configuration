namespace Flash.Configuration.Core;

/// <summary>
/// Mark a property
/// </summary>
/// <param name="displayName"></param>
/// <param name="isComplex"></param>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Property,  AllowMultiple = true)]
public sealed class FlashPropertyAttribute(string? displayName = null, bool isComplex = false) : Attribute
{
    public string? DisplayName { get; } = displayName;
    public bool IsComplex { get; } = isComplex;
}