using Flash.Configuration.Core;

namespace Flash.Configuration.TestConsole.Conf;

[FlashConfig(displayName: "CustomStagingConfig", environment: "Staging")]
public sealed class TestStagingConfig
{
    [FlashField] public readonly string TestVariableName = "Test variable value 01";
    [FlashField] [FlashDisable] public readonly string HideVariableName = "Hide variable value 01";
}