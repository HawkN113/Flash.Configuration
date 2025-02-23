using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Common.Handlers.CommandLineParser.Abstraction;
using Flash.Configuration.Common.Handlers.ConfigParser.Abstraction;
using Flash.Configuration.Common.Models;
using Flash.Configuration.Updater.Abstraction;
namespace Flash.Configuration.Updater;

internal sealed class AppHandler(
    IAppValidator appValidator,
    IConfigProcessor configProcessor,
    ICommandProcessor commandProcessor,
    IAssemblyProcessor assemblyProcessor) : IAppHandler
{
    /// <summary>
    /// Handle the process:
    ///     - Parse command arguments
    ///     - Validate arguments
    ///     - Get configuration data
    ///     - Get available configuration files
    ///     - Update configuration files
    /// </summary>
    public async Task HandleAsync(string[] args)
    {
        try
        {
            var options = await commandProcessor.ParseArgsAsync<ArgsOptions>(args);
            appValidator.Validate(options);

            var configData = assemblyProcessor.ParseAssembly(options.AssemblyPath);
            
            var projectConfigs = configProcessor.GetAvailableConfigs(options.ProjectPath);
            var outputConfigs = configProcessor.GetAvailableConfigs(options.ProjectOutputPath);

            await UpdateConfigsAsync(projectConfigs, configData, "project directory");
            await UpdateConfigsAsync(outputConfigs, configData, "output directory");
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync($"ERROR: {ex.Message}");
        }
    }

    private async Task UpdateConfigsAsync(
        ConfigDetails[] configs,
        Dictionary<string, Dictionary<string, object>> configData,
        string target)
    {
        await Console.Out.WriteLineAsync($"Processing configuration files in {target}:");
        if (configs.Length == 0 && configData.Count == 0)
        {
            await Console.Error.WriteLineAsync("\tWARNING: No configuration files or data found.");
            return;
        }

        var updateTasks = configs.Select(config => UpdateConfigAsync(config, configData)).ToList();

        await Task.WhenAll(updateTasks);

        var missingEnvironments =
            configData.Keys.Except(configs.Select(c => c.Environment), StringComparer.OrdinalIgnoreCase);
        foreach (var missingEnv in missingEnvironments)
            await Console.Error.WriteLineAsync(
                $"\tWARNING: No configuration file found for environment '{missingEnv.ToUpper()}'.");
    }

    private async Task UpdateConfigAsync(ConfigDetails config,
        Dictionary<string, Dictionary<string, object>> configData)
    {
        if (!configData.TryGetValue(config.Environment, out var values) || values.Count == 0)
        {
            await Console.Error.WriteLineAsync($"\tWARNING: Skipping '{config.FilePath}' (No matching data).");
            return;
        }

        var result = await configProcessor.UpdateConfigAsync(config, values);
        if (result.IsSuccessful)
            await Console.Out.WriteLineAsync($"\tINFO: The file '{config.FilePath}' was updated successfully.");
        if (result.IsFailed)
            await Console.Error.WriteLineAsync(
                $"\tERROR: Failed to update '{config.FilePath}': {result.ErrorMessages}");
    }
}