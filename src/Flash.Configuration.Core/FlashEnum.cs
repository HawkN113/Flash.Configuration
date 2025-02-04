namespace Flash.Configuration.Core;

/// <summary>
/// Enum will be converted to int
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = true)]
public sealed class FlashEnumAttribute : Attribute
{ }