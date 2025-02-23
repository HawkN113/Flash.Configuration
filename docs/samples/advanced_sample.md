### Classes in the project
#### [ApiKeysSettings.cs](../../src/samples/Flash.Configuration.WebApi/Configuration/ApiKeysSettings.cs)
```csharp
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
```

#### [AppSettings.cs](../../src/samples/Flash.Configuration.WebApi/Configuration/AppSettings.cs)
```csharp
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
```

#### [Connections.cs](../../src/samples/Flash.Configuration.WebApi/Configuration/Connections.cs)
```csharp
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
```

#### [ConnectionStrings.cs](../../src/samples/Flash.Configuration.WebApi/Configuration/ConnectionStrings.cs)
```csharp
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
```

#### [FeatureFlags.cs](../../src/samples/Flash.Configuration.WebApi/Configuration/FeatureFlags.cs)
```csharp
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
```

#### [FileLogging.cs](../../src/samples/Flash.Configuration.WebApi/Configuration/FileLogging.cs)
```csharp
using Flash.Configuration.Core;

namespace Flash.Configuration.WebApi.Configuration;

[FlashOrder(6)]
[FlashConfig("FileLogging")]
[FlashConfig("FileLogging", environment: "Staging")]
public class FileLogging
{
    [FlashProperty("Enabled")]
    [FlashValueIgnore(environment: "Staging")]
    public bool Enabled { get; set; } = true;

    [FlashProperty("LogFilePath")]
    [FlashValue("logs/app-stg-log.txt", environment: "Staging")]
    public string LogFilePath { get; set; } = "logs/app-log.txt";
}
```

#### [HighLevelConfig.cs](../../src/samples/Flash.Configuration.WebApi/Configuration/HighLevelConfig.cs)
```csharp
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
```

#### [OverrideConfig.cs](../../src/samples/Flash.Configuration.WebApi/Configuration/OverrideConfig.cs)
```csharp
using Flash.Configuration.Core;

namespace Flash.Configuration.WebApi.Configuration;

[FlashOrder(10)]
[FlashConfig("Override-Config")]
public class OverrideConfigFirst
{
    [FlashField("String-field")] public readonly string StringField = "Default field value";
    [FlashField("Int-field")] public const int IntField = 2;
}

[FlashOrder(11)]
[FlashConfig("Override-Config")]
public class OverrideLConfigSecond
{
    [FlashField("String-field")] public readonly string StringField = "Override default value. Use \'FlashOrder\' (last configuration is won)";
    [FlashField("Int-field")] public const int IntField = -2;
}
```

---

### Updated configuration files
- `appsettings.json`
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AppSettings": {
    "ApplicationName": "MyTutorialApp",
    "Version": "1.0.0",
    "EnableFeatureX": true,
    "MaxRetryCount": 3,
    "AllowedHosts": "*",
    "Environment": "",
    "MiddlewareSettings": {
      "Skip-urls": [
        "/health",
        "/live"
      ],
      "Health-urls": [
        "/health/ready",
        "/health/live"
      ]
    },
    "LoadBalancingType": "IPHashing",
    "Balancing": 2
  },
  "APIKeys": {
    "GoogleMaps": "your-google-maps-api-key",
    "OpenWeather": "your-openweather-api-key",
    "ApiKey": 1
  },
  "FileLogging": {
    "Enabled": true,
    "LogFilePath": "logs/app-log.txt"
  },
  "Override-Config": {
    "String-field": "Override default value. Use 'FlashOrder' (last configuration is won)",
    "Int-field": -2
  },
  "High-Level-Config": {
    "Custom-string-field": "field value",
    "Custom-bool-field": false,
    "Custom-bool-without-value-field": false,
    "Custom-int-field": 2,
    "Custom-double-field": 2,
    "Custom-float-field": 2.2,
    "Custom-string-property": "",
    "Calculated-int-property": 10,
    "Calculated-string-property": "Hello, World!"
  }
}
```

- `appsettings.Development.json`
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AppSettings": {
    "ApplicationName": "MyTutorialApp",
    "Version": "1.0.0",
    "EnableFeatureX": false,
    "MaxRetryCount": 2,
    "AllowedHosts": "*",
    "Environment": "Development",
    "Balancing": 2
  },
  "APIKeys": {
    "GoogleMaps": "your-dev-google-maps-api-key",
    "OpenWeather": "your-dev-openweather-api-key",
    "ApiKey": 2
  },
  "Connections": {
    "UserService": {
      "BaseUrl": "https://dev.api.localhost",
      "RelativeUrl": "api/users/v1",
      "SwaggerUrl": "api/users/v1"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=dev.localhost;Database=dev_db;User Id=dev_user;Password=******;"
  },
  "FeatureFlags": {
    "EnableBetaFeatures": false,
    "EnableNewUI": false
  }
}
```

- `appsettings.Staging.json`
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "AppSettings": {
    "ApplicationName": "MyTutorialApp",
    "Version": "1.0.0",
    "EnableFeatureX": true,
    "MaxRetryCount": 4,
    "AllowedHosts": "*",
    "Environment": "Staging",
    "LoadBalancingType": "RoundRobin",
    "Balancing": 3
  },
  "APIKeys": {
    "GoogleMaps": "your-staging-google-maps-api-key",
    "OpenWeather": "your-staging-openweather-api-key",
    "ApiKey": 3
  },
  "Connections": {
    "UserService": {
      "BaseUrl": "https://staging.api.localhost",
      "RelativeUrl": "api/users/v1",
      "SwaggerUrl": "api/users/v1"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=staging.localhost;Database=staging_db;User Id=staging_user;Password=******;"
  },
  "FeatureFlags": {
    "EnableBetaFeatures": true,
    "EnableNewUI": true
  },
  "FileLogging": {
    "LogFilePath": "logs/app-stg-log.txt"
  }
}
```