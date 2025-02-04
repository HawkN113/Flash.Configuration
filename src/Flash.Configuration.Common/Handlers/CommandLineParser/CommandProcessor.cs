using System.Reflection;
using System.Text.RegularExpressions;
using Flash.Configuration.Common.Handlers.CommandLineParser.Abstraction;
using Flash.Configuration.Common.Handlers.CommandLineParser.Attributes;
namespace Flash.Configuration.Common.Handlers.CommandLineParser;

public partial class CommandProcessor : ICommandProcessor
{
    [GeneratedRegex(@"--(?<keyId>\w+)(?:[ =](?<value>.+))?", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private partial Regex ArgsRegex();

    public async Task<T> ParseArgsAsync<T>(string[] args) where T : class, new()
    {
        var result = new Dictionary<string, string>();
        var regex = ArgsRegex();
        foreach (var arg in args)
        {
            var match = regex.Match(arg);
            if (!match.Success) continue;
            var key = match.Groups["keyId"].Value;
            var value = match.Groups["value"].Success ? match.Groups["value"].Value : "true";
            result[key] = value;
        }

        return await Task.FromResult(PopulateOptions<T>(result));
    }

    private static T PopulateOptions<T>(Dictionary<string, string> data) where T : new()
    {
        var options = new T();
        var type = typeof(T);

        foreach (var prop in type.GetProperties())
        {
            var aliasAttr = prop.GetCustomAttribute<PropertyAliasAttribute>();
            var propertyName = aliasAttr?.AliasName ?? prop.Name;

            if (!data.TryGetValue(propertyName, out var value)) continue;

            var convertedValue = Convert.ChangeType(value, prop.PropertyType);
            prop.SetValue(options, convertedValue);
        }

        return options;
    }
}