using Flash.Configuration.Core;

namespace Flash.Configuration.TestConsole.Conf;

[FlashConfig(displayName: "CustomDevConfig", environment: "Development")]
public class TestDevConfig
{
    [FlashProperty("DevString")] public string DevLevelString { get; } = "string-dev";
    [FlashProperty("DevInt")] public int DevLevelInt { get; } = 1;
    [FlashProperty("DevEnum0")] public Test DevLevelEnum0 { get; } = Test.Base;
    [FlashProperty("DevEnum1")] [FlashEnum] public Test DevLevelEnum1 { get; } = Test.None;
}

[FlashEnumString]
public enum Test
{
    None = 1,
    Base = 2
}