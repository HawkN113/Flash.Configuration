# Flash.Configuration

Flash.Configuration is a configuration management tool that enables dynamic parsing and updating of configuration settings, including updating configuration files after build.

## Features
- **Dynamic configuration parsing**: Supports parsing configuration files in JSON format. Extracts and structures configuration data for easy access.
- **Automatic configuration updates**: Updates configuration files dynamically based on specified rules. Ensures configurations remain up to date without manual intervention.
- **Post-Build configuration handling**: Automatically modifies configuration files after the application build process. Useful for adapting settings to different environments (e.g., development, staging, production).
- **Environment-specific configurations**: Manages different configuration files based on the target deployment environment. Allows seamless switching between settings for different use cases.
- **Validation and Error Handling**: Includes built-in validation mechanisms to detect and report misconfigurations. Logs errors and warnings for troubleshooting issues.
- **Extensibility and Customization**: Allows developers to define custom processing rules for configurations. Supports plug-in extensions to enhance functionality.

---

## Getting Started

### Installation

To add the latest [NuGet package](https://www.nuget.org/packages/Flash.Configuration):
```bash
Install-Package Flash.Configuration --version 8.0.0
```
or
```bash
dotnet add package Flash.Configuration --version 8.0.0
```
The package can work with the following project types:
- Console Application
- Web API
- Class Library
- Web API (native AOT)
- Worker service

---

### Prerequisites

- .NET 8 or higher.

---

### Usage

- Create a class for configuration

  **Sample**
```csharp
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
- Add the following configuration files to the project:
    - `appsettings.Development.json`
    - `appsettings.Staging.json`
- Build the project;
    - The configuration files should be updated:
        -  `appsettings.Development.json`
      ```json
      {
          "Logging": {
            "LogLevel": {
              "Default": "Information",
              "Microsoft.AspNetCore": "Warning"
            }
          },
            "ConnectionStrings": {
              "DefaultConnection": "Server=dev.localhost;Database=dev_db;User Id=dev_user;Password=******;"
          }
      }
      ```
        -  `appsettings.Staging.json`
      ```json
      {
          "Logging": {
            "LogLevel": {
              "Default": "Debug",
              "System": "Information",
              "Microsoft": "Information"
            }
          },
            "ConnectionStrings": {
              "DefaultConnection": "Server=staging.localhost;Database=staging_db;User Id=staging_user;Password=******;"
          }
      }
      ```

---

### Advanced usage

⚠️ **Warning** The advanced complex sample is available with the [link](https://github.com/HawkN113/Flash.Configuration/main/docs/samples/advanced_sample.md) with all classes and configuration files.
