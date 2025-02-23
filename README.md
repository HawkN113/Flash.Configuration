# Flash.Configuration

| ![Flash.Configuration](docs/img/flash.configuration.png) | Flash.Configuration is a configuration management tool that enables dynamic parsing and updating of configuration settings, including updating configuration files after build. |
|----------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

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

To add the Nuget package for the project type.

| Package                                                                                     | Project type                                                                                         |
|---------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------|
| [Flash.Configuration](https://www.nuget.org/packages/Flash.Configuration)                   | Console Application,<br/> Web API,<br/> Class Library,<br/> Web API (native AOT),<br/>Worker service |
| [Flash.Configuration.Wpf](https://www.nuget.org/packages/Flash.Configuration.Wpf)           | WPF Application,<br/> WPF Class Library                                                              |
| [Flash.Configuration.WinForms](https://www.nuget.org/packages/Flash.Configuration.WinForms) | Windows Forms App,<br/> Windows Forms Class Library                                                  |

#### Flash.Configuration
To add the latest [NuGet package](https://www.nuget.org/packages/Flash.Configuration):
```bash
Install-Package Flash.Configuration --version 8.0.0
```
or
```bash
dotnet add package Flash.Configuration --version 8.0.0
```

#### Flash.Configuration.Wpf
To add the latest [NuGet package](https://www.nuget.org/packages/Flash.Configuration.Wpf):
```bash
Install-Package Flash.Configuration.Wpf --version 8.0.0
```
or
```bash
dotnet add package Flash.Configuration.Wpf --version 8.0.0
```

#### Flash.Configuration.WinForms
To add the latest [NuGet package](https://www.nuget.org/packages/Flash.Configuration.WinForms):
```bash
Install-Package Flash.Configuration.WinForms --version 8.0.0
```
or
```bash
dotnet add package Flash.Configuration.WinForms --version 8.0.0
```

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
- More samples are available with the [link](src/samples)

---

### Advanced usage

⚠️ **Warning** The advanced complex sample is available with the [link](docs/samples/advanced_sample.md) with all classes and configuration files.

#### Main attributes

- `FlashConfig` marks high-level configuration section for the environments:
    - Empty - should be applied the changes into file 'appSettings.json' <br/>
    - Development - should be applied the changes into file 'appSettings.Development.json' <br/>
    - Staging - should be applied the changes into file 'appSettings.Staging.json' <br/>
    - Production - should be applied the changes into file 'appSettings.Production.json'
    - You can use an other configuration name, i.e: `Test`, `Qa`
   **Samples**
   ```csharp
   [FlashConfig("ConnectionStrings", environment: "Development")]
   ```
   Main section with name `ConnectionStrings` will be available on `Development` environment

   ```csharp
   [FlashConfig("ConnectionStrings")]
   ```
  Main section with name `ConnectionStrings` will be available for all environments (will be updated only `appsettings.json`)

- `FlashProperty` marks a property in configuration section for the environments.
   **Samples**
   ```csharp
   [FlashProperty("DefaultConnection")]
   ```
  The property has the name `DefaultConnection` will be available for all environments
   ```csharp
   [FlashProperty("MiddlewareSettings", isComplex: true)]
   ```
  The property has the name `MiddlewareSettings` will be available for all environments and use the complex configuration. In the case, should use attribute `FlashSection`

- `FlashSection` marks the complex configuration for the property in configuration section.
  **Samples**
   ```csharp
   [FlashSection]
   public class Settings
   {
      [FlashProperty("Skip-urls")] public required string[] SkipUrls { get; set; }
      [FlashProperty("Health-urls")] public required string[] HealthUrls { get; set; }
   }
   ```
  The complex section will be available for the property

- `FlashValue` allows set the default property for the property for the environments.
  **Samples**
   ```csharp
   [FlashValue("Server=dev.localhost;Database=dev_db;User Id=dev_user;Password=******;", environment: "Development")]
   ```
   The value will be available on `Development` environment.

- `FlashValueIgnore` allows to ignore the default property for the property for the environments.
  **Samples**
   ```csharp
   [FlashValueIgnore("Development")]
   ```
   The value will be ignored on `Development` environment.

- `FlashIgnore` allows to ignore the section/property/field/class on the environments. Available for all components
  **Samples**
   ```csharp
   [FlashIgnore]
   ```

- `FlashField` marks a field in configuration section for the environments.
  **Samples**
   ```csharp
   [FlashField("DefaultConnection")]
   ```
   The field has the name `DefaultConnection` will be available for all environments
   ```csharp
   [FlashField("MiddlewareSettings")]
   ```
   The field has the name `MiddlewareSettings` will be available for all environments and use the complex configuration.

- `FlashOrder` allows to set order of a component in configuration section for the environments.
  **Samples**
   ```csharp
   [FlashOrder(1)]
   ```
  The component will be set first in the configuration for the environment

---

## License

This project is licensed under the MIT License.