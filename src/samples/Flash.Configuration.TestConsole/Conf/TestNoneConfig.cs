using Flash.Configuration.Core;

namespace Flash.Configuration.TestConsole.Conf;

[FlashConfig(displayName:"Test-None-Config")]
public class TestNoneConfig
{
    [FlashProperty("TestString01")] public string TestString0 { get; set; } = "string";
    [FlashProperty] public int TestInt0 { get; set; } = 1;

    [FlashProperty("TestSection-01", isComplex: true)]
    public TestSection TestSection01 { get; set; } = new TestSection()
        { TestStringSection1 = "TestString1", TestIntSection1 = 1235 };

    [FlashProperty(isComplex: true)]
    public TestSection TestSection02 { get; set; } = new TestSection()
        { TestStringSection1 = "TestString2", TestIntSection1 = 1234 };

    public void TestMethod()
    {
    }

    public string TestHideProperty1 { get; set; }
}

[FlashSection]
public class TestSection
{
    [FlashProperty] public string TestStringSection1 { get; set; }
    [FlashProperty] public int TestIntSection1 { get; set; }
    [FlashDisable]
    [FlashProperty] public int TestIntSection12 { get; set; }
}