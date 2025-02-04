using Flash.Configuration.Abstraction;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Common.Handlers.CommandLineParser.Abstraction;
using Flash.Configuration.Common.Handlers.ConfigParser.Abstraction;
using Flash.Configuration.Common.Models;
using Flash.Configuration.Configuration;
namespace Flash.Configuration;

internal class AppHandler(
    IConfigProcessor configProcessor,
    ICommandProcessor commandProcessor,
    IAssemblyProcessor assemblyProcessor) : IAppHandler
{
    /// <summary>
    /// Handle the process
    ///     - Parse command arguments
    ///     - Get configuration data
    ///     - Get available configuration files
    ///     - Update configuration files
    /// </summary>
    /// <param name="args"></param>
    public async Task HandleAsync(string[] args)
    {
        // -------------------------- Parse command args --------------------------
        // Get a path to assembly name (dll) using the following command arguments: 
        //      --proj_path - the path to project directory
        //      --output_path - the path to Debug/Release folder
        //      --assembly_name - the name of the assembly with extension .dll
        var options = await commandProcessor.ParseArgsAsync<Options>(args);

        if (string.IsNullOrEmpty(options.ProjectPath) && string.IsNullOrEmpty(options.OutputPath) &&
            string.IsNullOrEmpty(options.AssemblyName))
            throw new InvalidOperationException("Could not find command arguments.");

        // Get full absolute path to the assembly
        var pathToDll = Path.Combine(options.ProjectPath, options.OutputPath, options.AssemblyName);

        if (options.Verbose)
        {
            Console.WriteLine($"Project path: '{options.ProjectPath}' \n" +
                              $"Output path: '{options.OutputPath}' \n" +
                              $"Assembly name: '{options.AssemblyName}' \n" +
                              $"Full path to assembly: '{pathToDll}'");
        }

        // -------------------------- Get configuration data--------------------------
        // Get reflected classes with attributes 'FlashConfig'
        var configData = await assemblyProcessor.ParseAssemblyAsync(pathToDll);

        // -------------------------- Get available configuration files--------------------------
        // Get all available configuration files in project and output directories
        var projectConfigs = configProcessor.GetAvailableConfigs(options.ProjectPath);
        var outputConfigs = configProcessor.GetAvailableConfigs(Path.Combine(options.ProjectPath, options.OutputPath));

        // -------------------------- Update configuration files--------------------------
        await UpdatedConfigsAsync(projectConfigs, configData, "project directory");
        await UpdatedConfigsAsync(outputConfigs, configData, "output directory");
    }

    private async Task UpdatedConfigsAsync(IEnumerable<ConfigDetails> configs,
        Dictionary<string, Dictionary<string, object>> configData, string target)
    {
        Console.WriteLine($"Processing files in {target}:");
        var configDetailsList = configs as ConfigDetails[] ?? configs.ToArray();
        if (configDetailsList.Length == 0)
        {
            Console.WriteLine($"\tWARNING: Could not find files.");
        }

        foreach (var config in configDetailsList)
        {
            if (configData.TryGetValue(config.Environment, out Dictionary<string, object>? value) && value.Count > 0)
            {
                var result = await configProcessor.UpdateConfigAsync(config, value);
                Console.WriteLine(
                    result.IsSuccess
                        ? $"\tThe file '{config.FilePath}' was updated successfully."
                        : $"\tERROR: The file '{config.FilePath}': {result.Error}");
            }
            else
            {
                Console.WriteLine($"\tWARNING: Skip updating the file '{config.FilePath}'.");
            }
        }

        foreach (var configEnv in configData.Where(configEnv =>
                     configDetailsList.All(s => s.Environment != configEnv.Key)))
        {
            Console.WriteLine($"\tWARNING: Could not find file for environment '{configEnv.Key.ToUpper()}'.");
        }
    }
}