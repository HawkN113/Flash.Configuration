namespace Flash.Configuration.Common.Handlers.CommandLineParser.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class PropertyAliasAttribute(string aliasName) : Attribute
{
    public string AliasName { get; } = aliasName;
}