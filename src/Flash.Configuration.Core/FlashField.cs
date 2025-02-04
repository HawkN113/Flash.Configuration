namespace Flash.Configuration.Core;

/// <summary>
/// Mark a field
/// </summary>
/// <param name="displayName"></param>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Field,  AllowMultiple = true)]
public sealed class FlashFieldAttribute(string? displayName = null) : Attribute
{
    public string? DisplayName { get; } = displayName;
}