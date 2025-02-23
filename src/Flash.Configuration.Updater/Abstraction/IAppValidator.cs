using Flash.Configuration.Common.Models;
namespace Flash.Configuration.Updater.Abstraction;

/// <summary>
/// Application validator
/// </summary>
public interface IAppValidator
{
    /// <summary>
    /// Validate command line arguments
    /// </summary>
    /// <param name="options"></param>
    void Validate(ArgsOptions options);
}