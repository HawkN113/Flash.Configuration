using System.Text.RegularExpressions;

namespace Flash.Configuration.Common.Helpers;

public static partial class RegExpressions
{
    [GeneratedRegex(@"^appsettings(?:\.(.+))?\.json$", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    public static partial Regex ConfigFileRegex();

    [GeneratedRegex(@"--(?<keyId>\w+)(?:[ =](?<value>.+))?", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    public static partial Regex ArgsRegex();

    [GeneratedRegex(@"(?i)\.dll$", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    public static partial Regex AssemblyNameRegex();

    [GeneratedRegex(@"^(?:[a-zA-Z]:\\|\/|\\\\)", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    public static partial Regex ProjectPathRegex();
}