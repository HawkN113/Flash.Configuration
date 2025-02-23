namespace Flash.Configuration.Core;

/// <summary>
/// Mark configuration section for environment
/// </summary>
/// <param name="displayName">Display name of section in configuration</param>
/// <param name="environment">Use environment name for the configuration: <br/>
///     Empty - should be applied the changes into file 'appSettings.json' <br/>
///     Development - should be applied the changes into file 'appSettings.Development.json' <br/>
///     Staging - should be applied the changes into file 'appSettings.Staging.json' <br/>
///     Production - should be applied the changes into file 'appSettings.Production.json'
/// </param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class FlashConfigAttribute(string? displayName = null, string? environment = null)
    : Attribute
{
    /// <summary>
    /// Environment
    /// </summary>
    public string? Environment { get; } = environment;
    /// <summary>
    /// Display name
    /// </summary>
    public string? DisplayName { get; } = displayName;
}