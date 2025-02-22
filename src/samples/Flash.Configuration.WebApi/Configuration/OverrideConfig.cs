using Flash.Configuration.Core;

namespace Flash.Configuration.WebApi.Configuration;

[FlashOrder(10)]
[FlashConfig("Override-Config")]
public class OverrideConfigFirst
{
    [FlashField("String-field")] public readonly string StringField = "Default field value";
    [FlashField("Int-field")] public const int IntField = 2;
}

[FlashOrder(11)]
[FlashConfig("Override-Config")]
public class OverrideLConfigSecond
{
    [FlashField("String-field")] public readonly string StringField = "Override default value. Use \'FlashOrder\' (last configuration is won)";
    [FlashField("Int-field")] public const int IntField = -2;
}