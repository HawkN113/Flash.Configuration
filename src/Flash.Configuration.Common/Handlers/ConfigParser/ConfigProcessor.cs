using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using Flash.Configuration.Common.Handlers.ConfigParser.Abstraction;
using Flash.Configuration.Common.Models;
namespace Flash.Configuration.Common.Handlers.ConfigParser;

public sealed partial class ConfigProcessor : IConfigProcessor
{
    private const string ConfigFilesPattern = @"^appsettings(?:\.(.+))?\.json$";

    [GeneratedRegex(ConfigFilesPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex ConfigFilesRegex();

    public ConfigDetails[] GetAvailableConfigs(string projectPath)
    {
        if (!Directory.Exists(projectPath))
        {
            return Array.Empty<ConfigDetails>();
        }

        return Directory.EnumerateFiles(projectPath, "*.json", SearchOption.TopDirectoryOnly)
            .Where(file => ConfigFilesRegex().IsMatch(Path.GetFileName(file)))
            .Select(file => new ConfigDetails { FilePath = file })
            .Where(config => config.Environment != "Invalid")
            .ToArray();
    }

    public async Task<Result> UpdateConfigAsync(ConfigDetails configDetails, Dictionary<string, object>? configData)
    {
        if (!File.Exists(configDetails.FilePath))
        {
            return Result.Failure($"Config file '{configDetails.FilePath}' not found.");
        }

        if (configData is null || configData.Count == 0)
        {
            return Result.Failure("No configuration data provided.");
        }

        try
        {
            var sections = configData
                .Where(pair => pair.Value is Dictionary<string, object>)
                .ToDictionary(pair => pair.Key, pair => ConvertToJsonObject((Dictionary<string, object>)pair.Value));

            if (sections.Count == 0)
            {
                return Result.Failure("Invalid configuration structure.");
            }

            await UpdateJsonFileAsync(configDetails.FilePath, sections);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Configuration update failed: {ex.Message}");
        }
    }

    private async Task UpdateJsonFileAsync(string filePath, Dictionary<string, JsonObject> sectionData)
    {
        try
        {
            var jsonContent = await File.ReadAllTextAsync(filePath);
            var root = JsonNode.Parse(jsonContent) as JsonObject ?? new JsonObject();

            foreach (var (key, newData) in sectionData)
            {
                root[key] = newData;
            }

            var updatedJson = root.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            });

            await File.WriteAllTextAsync(filePath, updatedJson);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to update JSON file: {filePath}", ex);
        }
    }

    private static JsonObject ConvertToJsonObject(Dictionary<string, object> dictionary)
    {
        var jsonObject = new JsonObject();

        foreach (var (key, value) in dictionary)
        {
            jsonObject[key] = value is Dictionary<string, object> nestedDict
                ? ConvertToJsonObject(nestedDict)
                : JsonValue.Create(value);
        }

        return jsonObject;
    }
}