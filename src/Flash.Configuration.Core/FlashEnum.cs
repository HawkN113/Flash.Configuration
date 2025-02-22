namespace Flash.Configuration.Core;

/// <summary>
/// Mark 'Enum' property type for configuration. Enum value won't be converted to 'String' automatically.
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = true)]
public sealed class FlashEnumAttribute : Attribute;