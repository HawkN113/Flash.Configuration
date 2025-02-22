using Flash.Configuration.Core;

namespace Flash.Configuration.WebApi.Configuration;

[FlashOrder(3)]
[FlashConfig("Connections", environment: "Development")]
[FlashConfig("Connections", environment: "Staging")]
public class Connections
{
    [FlashProperty("UserService", isComplex: true)]
    public UserService UserService { get; } = new();
}

[FlashSection]
public class UserService
{
    [FlashProperty("BaseUrl")]
    [FlashValue("https://dev.api.localhost", environment: "Development")]
    [FlashValue("https://staging.api.localhost", environment: "Staging")]
    public string BaseUrl { get; set; } = string.Empty;

    [FlashProperty("RelativeUrl")]
    [FlashValue("api/users/v1", environment: "Development")]
    [FlashValue("api/users/v1", environment: "Staging")]
    public string RelativeUrl { get; set; } = string.Empty;

    [FlashProperty("SwaggerUrl")]
    [FlashValue("api/users/v1", environment: "Development")]
    [FlashValue("api/users/v1", environment: "Staging")]
    public string SwaggerUrl { get; set; } = string.Empty;

    public string GetFullUrl() => string.Concat(BaseUrl, "/", RelativeUrl);
}