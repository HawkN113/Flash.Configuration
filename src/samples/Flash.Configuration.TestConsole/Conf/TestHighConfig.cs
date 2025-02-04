using Flash.Configuration.Core;

namespace Flash.Configuration.TestConsole.Conf;

[FlashConfig(displayName:"CustomHighConfig")]
[FlashConfig(displayName:"CustomHighConfig", environment: "Staging")]
public class TestHighConfig
{
    [FlashProperty("HighString")] public string HighLevelString { get; set; } = "string-high";
    [FlashProperty(displayName:"HighInt")] public int HighLevelInt { get; } = 0; 

    [FlashDisable]
    [FlashProperty] public string HideProperty { get; set; } = "You should not see this!";

    public string IgnoredProperty01 { get; set; } = "You should see this!";
}