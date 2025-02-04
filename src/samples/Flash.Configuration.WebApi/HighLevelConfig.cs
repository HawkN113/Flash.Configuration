using Flash.Configuration.Core;

namespace Flash.Configuration.WebApi;

[FlashConfig("Dev-Level-Config", environment: "Development")]
public class HighLevelConfig
{
    [FlashProperty("Property-01")] public string Property01 { get; } = "property01";

    [FlashField("Field-02")] public int Field02 = 2;

    [FlashProperty("Property-03")] public int Property03 { get; } = GetCalculateProperty();
    
    private static int GetCalculateProperty() => 2 + 3;
}