using Flash.Configuration.Core;

namespace Flash.Configuration.WebApi.Configuration;

[FlashConfig("High-Level-Config")]
public class HighLevelConfig0
{
    [FlashField("Custom-string-field")] public readonly string CustomStringField = "field value";
    [FlashField("Custom-int-field")] public const int CustomIntField = 2;
    [FlashField("Custom-double-field")] public const double CustomDoubleField = 2.0;
    [FlashField("Custom-float-field")] public const float CustomFloatField = 2.2f;
    [FlashField("Custom-bool-field")] public bool CustomBoolField;
    [FlashField("Custom-bool-without-value-field")] public bool CustomBoolFieldWithoutValue;
    [FlashProperty("Custom-string-property")]
    public string CustomStringProperty { get; } = "";
    [FlashProperty("Custom-string-declare-property")]
    // ReSharper disable once UnusedMember.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public string CustomDeclareStringProperty { get; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    [FlashProperty("Calculated-int-property")]
    public int IntProperty { get; } = GetCalculateProperty();
    [FlashProperty("Calculated-string-property")]
    public string StringProperty { get; } = "Hello, World!";
    private static int GetCalculateProperty() => (2 + 3) * 2;
}