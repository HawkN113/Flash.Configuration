using Flash.Configuration.Core;

namespace Flash.Configuration.Console.Configuration;

[FlashConfig("Console-CommonConfig")]
public class CommonConfig
{
    [FlashProperty("Property1")]
    [FlashValue("Property1_value")]
    public string Property1 { get; set; } = "Default value";
}