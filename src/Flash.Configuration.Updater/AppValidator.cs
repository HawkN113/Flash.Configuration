using Flash.Configuration.Common.Models;
using Flash.Configuration.Updater.Abstraction;
namespace Flash.Configuration.Updater;

internal sealed class AppValidator : IAppValidator
{
    public void Validate(ArgsOptions options)
    {
        if (options is null)
            throw new InvalidOperationException("Command line arguments are required.");
        if (string.IsNullOrEmpty(options.ProjectPath) ||
            !Common.Helpers.RegExpressions.ProjectPathRegex().IsMatch(options.ProjectPath))
            throw new InvalidOperationException($"Project path is invalid: {options.ProjectPath}");
        if (string.IsNullOrWhiteSpace(options.OutputPath))
            throw new InvalidOperationException("Output path is required.");
        if (string.IsNullOrEmpty(options.AssemblyName) ||
            !Common.Helpers.RegExpressions.AssemblyNameRegex().IsMatch(options.AssemblyName))
            throw new InvalidOperationException($"Assembly name is invalid: {options.AssemblyName}");
    }
}