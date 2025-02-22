using Flash.Configuration.Core;

namespace Flash.Configuration.WebApi.Configuration;

[FlashOrder(2)]
[FlashConfig("APIKeys")]
[FlashConfig("APIKeys", environment: "Development")]
[FlashConfig("APIKeys", environment: "Staging")]
public class ApiKeys
{
    [FlashField("GoogleMaps")]
    [FlashValue("your-google-maps-api-key")]
    [FlashValue("your-dev-google-maps-api-key", environment: "Development")]
    [FlashValue("your-staging-google-maps-api-key", environment: "Staging")]
    public readonly string GoogleMaps = string.Empty;

    [FlashField("OpenWeather")]
    [FlashValue("your-openweather-api-key")]
    [FlashValue("your-dev-openweather-api-key", environment: "Development")]
    [FlashValue("your-staging-openweather-api-key", environment: "Staging")]
    public readonly string OpenWeather = string.Empty;
    
    [FlashField("ApiKey")]
    [FlashValue(1)]
    [FlashValue(2, environment: "Development")]
    [FlashValue(3, environment: "Staging")]
    public readonly int ApiKey = 0;
}