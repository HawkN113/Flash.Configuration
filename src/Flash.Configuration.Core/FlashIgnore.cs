namespace Flash.Configuration.Core;

/// <summary>
/// Ignore (skip) visibility 'Property' or 'Field' or 'Class' for configuration. The element won't be processed automatically.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public sealed class FlashIgnoreAttribute : Attribute;