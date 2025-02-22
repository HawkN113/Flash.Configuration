namespace Flash.Configuration.Common.Models;

public sealed class ConfigDetails
{
    public required string FilePath { get; set; }

    public string Environment
    {
        get
        {
            var match = Helpers.RegExpressions.ConfigFileRegex().Match(Path.GetFileName(FilePath));
            if (match.Success)
                return match.Groups[1].Success ? match.Groups[1].Value.ToLower() : "None".ToLower();
            return "Invalid".ToLower();
        }
    }
}