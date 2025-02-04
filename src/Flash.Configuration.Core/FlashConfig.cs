namespace Flash.Configuration.Core;

/// <summary>
/// Mark configuration for environment
/// </summary>
/// <param name="environment">
///     Empty - should be applied the changes into file appSettings.json <br/>
///     Development - should be applied the changes into file appSettings.Development.json <br/>
///     Staging - should be applied the changes into file appSettings.Staging.json <br/>
///     Production - should be applied the changes into file appSettings.Production.json
/// </param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class FlashConfigAttribute(string? displayName = null, string? environment = null)
    : Attribute
{
    public string? Environment { get; } = environment;
    public string? DisplayName { get; } = displayName;
}