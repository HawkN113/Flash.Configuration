using Flash.Configuration.Common.Models;
namespace Flash.Configuration.Common.Handlers.ConfigParser.Abstraction;

public interface IConfigProcessor
{
    /// <summary>
    /// Get available configuration with the directory path
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <returns></returns>
    ConfigDetails[] GetAvailableConfigs(string directoryPath);
    
    /// <summary>
    /// Update configuration file with data
    /// </summary>
    /// <param name="configDetails"></param>
    /// <param name="configData"></param>
    /// <returns></returns>
    Task<Result> UpdateConfigAsync(ConfigDetails configDetails, Dictionary<string, object>? configData);
}