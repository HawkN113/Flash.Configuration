using Flash.Configuration.Common.Models;
namespace Flash.Configuration.Common.Handlers.ConfigParser.Abstraction;

public interface IConfigProcessor
{
    ConfigDetails[] GetAvailableConfigs(string projectPath);
    Task<Result> UpdateConfigAsync(ConfigDetails configDetails, Dictionary<string, object>? configData);
}