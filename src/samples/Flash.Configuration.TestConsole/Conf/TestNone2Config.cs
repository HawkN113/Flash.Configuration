using Flash.Configuration.Core;

namespace Flash.Configuration.TestConsole.Conf;

[FlashConfig(displayName:"Test-None-Config")]
public class TestNone2Config
{
    [FlashProperty("TestString010")] public string TestString0 { get; set; } = "string0";
    [FlashProperty] public int TestInt0 { get; set; } = 1;

    [FlashProperty("TestSection-010", isComplex: true)]
    public TestSection TestSection01 { get; set; } = new TestSection()
        { TestStringSection1 = "TestString10", TestIntSection1 = 120 };

    [FlashProperty(isComplex: true)]
    public TestSection TestSection02 { get; set; } = new TestSection()
        { TestStringSection1 = "TestString20", TestIntSection1 = 1230 };
}

[FlashSection]
public class TestSection2
{
    [FlashProperty] public string TestStringSection1 { get; set; }
    [FlashProperty] public int TestIntSection1 { get; set; }
}