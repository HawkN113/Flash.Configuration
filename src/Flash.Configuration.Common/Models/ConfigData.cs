using System.Text.RegularExpressions;
namespace Flash.Configuration.Common.Models;

public sealed partial class ConfigDetails
{
    public required string FilePath { get; set; }

    public string Environment
    {
        get
        {
            var match = ConfigFileRegex().Match(Path.GetFileName(FilePath));
            if (match.Success)
            {
                return match.Groups[1].Success ? match.Groups[1].Value.ToLower() : "None".ToLower();
            }

            return "Invalid".ToLower();
        }
    }

    [GeneratedRegex(@"^appsettings(?:\.(.+))?\.json$", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex ConfigFileRegex();
}