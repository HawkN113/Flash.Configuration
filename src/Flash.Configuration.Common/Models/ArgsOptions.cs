using Flash.Configuration.Common.Handlers.CommandLineParser.Attributes;

namespace Flash.Configuration.Common.Models;

/// <summary>
/// Command line arguments
/// </summary>
public sealed class ArgsOptions
{
    /// <summary>
    /// Project path. Should be set automatically during build process
    /// </summary>
    [PropertyAlias("proj_path")] public string ProjectPath { get; init; } = string.Empty;
    
    /// <summary>
    /// Output path (i.e: 'bin\Debug\net8.0\' or 'bin\Release\net8.0\'). Should be set automatically during build process
    /// </summary>
    [PropertyAlias("output_path")] public string OutputPath { get; init; } = string.Empty;
    
    /// <summary>
    /// File name of assembly (.dll)
    /// </summary>
    [PropertyAlias("assembly_name")] public string AssemblyName { get; init; } = string.Empty;

    public string ProjectOutputPath => Path.Combine(ProjectPath, OutputPath);

    public string AssemblyPath => Path.Combine(ProjectPath, OutputPath, AssemblyName);
}