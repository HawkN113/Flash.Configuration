namespace Flash.Configuration.Core;

/// <summary>
/// Order of configuration section for environment
/// </summary>
/// <param name="order">Order of section in configuration</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class FlashOrderAttribute(int order = int.MaxValue)
    : Attribute
{
    /// <summary>
    /// Order number
    /// </summary>
    public int Order { get; } = order;
}