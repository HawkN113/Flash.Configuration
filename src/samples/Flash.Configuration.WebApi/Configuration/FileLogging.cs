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