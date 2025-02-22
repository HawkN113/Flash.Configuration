using Flash.Configuration.Core;

namespace Flash.Configuration.WebApi.Configuration;

[FlashOrder(5)]
[FlashConfig("FeatureFlags", environment: "Development")]
[FlashConfig("FeatureFlags", environment: "Staging")]
public class FeatureFlags
{
    [FlashProperty("EnableBetaFeatures")]
    [FlashValue(false, environment: "Development")]
    [FlashValue(true, environment: "Staging")]
    public bool EnableBetaFeatures { get; set; } = true;
    
    [FlashProperty("EnableNewUI")]
    [FlashValue(false, environment: "Development")]
    public bool EnableNewUi { get; set; } = true;
}