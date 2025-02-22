namespace Flash.Configuration.Core;

/// <summary>
/// Mark complex object for the property. Used with related attribute 'FlashProperty'
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class FlashSectionAttribute : Attribute;