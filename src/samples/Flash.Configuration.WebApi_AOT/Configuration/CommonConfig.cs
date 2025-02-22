using Flash.Configuration.Core;

namespace Flash.Configuration.WebApi_AOT.Configuration;

[FlashConfig("WebApi-AOT-CommonConfig")]
[FlashConfig("WebApi-AOT-CommonConfig", environment: "Development")]
public class CommonConfig
{
    [FlashProperty("Property1")]
    [FlashValue("Property1_value")]
    public string Property1 { get; set; } = "Default value";
}