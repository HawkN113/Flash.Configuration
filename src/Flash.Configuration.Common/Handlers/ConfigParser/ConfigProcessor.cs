using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Flash.Configuration.Common.Handlers.ConfigParser.Abstraction;
using Flash.Configuration.Common.Models;
using Flash.Configuration.Common.Providers.Abstraction;

namespace Flash.Configuration.Common.Handlers.ConfigParser;

internal sealed class ConfigProcessor(IDirectoryProvider directoryProvider, IFileProvider fileProvider) : IConfigProcessor
{
    public ConfigDetails[] GetAvailableConfigs(string directoryPath)
    {
        if (!directoryProvider.Exists(directoryPath))
            return [];

        return directoryProvider.EnumerateFiles(directoryPath, "*.json", SearchOption.TopDirectoryOnly)
            .Where(file => Helpers.RegExpressions.ConfigFileRegex().IsMatch(Path.GetFileName(file)))
            .Select(file => new ConfigDetails { FilePath = file })
            .Where(config => config.Environment != "Invalid")
            .ToArray();
    }

    public async Task<Result> UpdateConfigAsync(ConfigDetails configDetails, Dictionary<string, object>? configData)
    {
        if (!fileProvider.Exists(configDetails.FilePath))
            return Result.Fail(new Error($"Config file '{configDetails.FilePath}' not found."));
        if (configData is null || configData.Count == 0)
            return Result.Fail(new Error("No configuration data provided."));

        try
        {
            var sections = configData
                .Where(pair => pair.Value is Dictionary<string, object>)
                .ToDictionary(pair => pair.Key, pair => ConvertToJsonObject((Dictionary<string, object>)pair.Value));

            await UpdateJsonFileAsync(configDetails.FilePath, sections);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Exception while updating '{configDetails.FilePath}': {ex.Message}"));
        }
    }

    private async Task UpdateJsonFileAsync(string filePath, Dictionary<string, JsonObject> sectionData)
    {
        try
        {
            var jsonContent = await fileProvider.ReadAllTextAsync(filePath);
            var root = JsonNode.Parse(jsonContent) as JsonObject ?? new JsonObject();
            foreach (var (key, newData) in sectionData)
                root[key] = newData;
            var updatedJson = root.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            await fileProvider.WriteAllTextAsync(filePath, updatedJson);
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