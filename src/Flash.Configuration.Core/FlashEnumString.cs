namespace Flash.Configuration.Core;

/// <summary>
/// Mark 'Enum' type for configuration. Enum value will be converted to 'String' value automatically
/// </summary>
[AttributeUsage(AttributeTargets.Enum, AllowMultiple = true)]
public sealed class FlashEnumStringAttribute : Attribute;