namespace Flash.Configuration.Core;

/// <summary>
/// Disable for configuration
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public sealed class FlashDisableAttribute : Attribute
{ }