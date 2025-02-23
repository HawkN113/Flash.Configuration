using Flash.Configuration.Core;

namespace Flash.Configuration.WebApi.Configuration;

[FlashOrder(4)]
[FlashConfig("ConnectionStrings", environment: "Development")]
[FlashConfig("ConnectionStrings", environment: "Staging")]
public class ConnectionStrings
{
    [FlashProperty("DefaultConnection")]
    [FlashValue("Server=dev.localhost;Database=dev_db;User Id=dev_user;Password=******;", environment: "Development")]
    [FlashValue("Server=staging.localhost;Database=staging_db;User Id=staging_user;Password=******;",
        environment: "Staging")]
    public string DefaultConnection { get; } = string.Empty;
    
    [FlashIgnore]
    [FlashProperty("Enabled")]
    [FlashValue(false, environment: "Development")]
    [FlashValue(true, environment: "Staging")]
    public bool Enabled { get; }
}