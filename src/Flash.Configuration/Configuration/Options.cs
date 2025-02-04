using Flash.Configuration.Common.Handlers.CommandLineParser.Attributes;
namespace Flash.Configuration.Configuration;

internal class Options
{
    [PropertyAlias("proj_path")] public string ProjectPath { get; init; } = string.Empty;
    [PropertyAlias("output_path")] public string OutputPath { get; init; } = string.Empty;
    [PropertyAlias("assembly_name")] public string AssemblyName { get; init; } = string.Empty;
    [PropertyAlias("verbose")] public bool Verbose { get; init; } = false;
}