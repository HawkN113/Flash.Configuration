using Flash.Configuration.Core;

namespace Flash.Configuration.WebApi.Configuration;

[FlashOrder(1)]
[FlashConfig("AppSettings")]
[FlashConfig("AppSettings", environment: "Development")]
[FlashConfig("AppSettings", environment: "Staging")]
public class AppSettings
{
    [FlashField("ApplicationName")] public readonly string ApplicationName = "MyTutorialApp";
    [FlashField("Version")] public readonly string Version = "1.0.0";
    
    [FlashProperty("Environment")]
    [FlashValue("Development", "Development")]
    [FlashValue("Staging", "Staging")]
    public string Environment { get; set; } = string.Empty;
    
    [FlashField("EnableFeatureX")] [FlashValue(false, "Development")] [FlashValue(true, "Staging")]
    public readonly bool EnableFeatureX = true;
    
    [FlashField("MaxRetryCount")] [FlashValue(2, "Development")] [FlashValue(4, "Staging")]
    public readonly int MaxRetryCount = 3;
    
    [FlashField("AllowedHosts")] public readonly string AllowedHosts = "*";
    
    [FlashProperty("MiddlewareSettings", isComplex: true)]
    [FlashValueIgnore("Development")]
    [FlashValueIgnore("Staging")]
    public Settings MiddlewareSettings { get; } = new()
    {
        SkipUrls = ["/health", "/live"],
        HealthUrls = ["/health/ready", "/health/live"]
    };

    [FlashProperty("LoadBalancingType")]
    [FlashValueIgnore("Development")]
    [FlashValue(LoadBalancingType.RoundRobin, "Staging")]
    public LoadBalancingType LoadBalancingType { get; } = LoadBalancingType.IPHashing;
    
    [FlashProperty("Balancing")]
    [FlashValue(LoadBalancingType.LeastConnections, "Development")]
    [FlashValue(LoadBalancingType.Weighted, "Staging")]
    [FlashEnum]
    public LoadBalancingType Balancing { get; } = LoadBalancingType.LeastConnections;
}

[FlashSection]
public class Settings
{
    [FlashProperty("Skip-urls")] public required string[] SkipUrls { get; set; }

    [FlashProperty("Health-urls")] public required string[] HealthUrls { get; set; }
}

[FlashEnumString]
public enum LoadBalancingType
{
    RoundRobin = 1,
    LeastConnections = 2,
    Weighted = 3,
    IPHashing = 4,
    Random = 5
}