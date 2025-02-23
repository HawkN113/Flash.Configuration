using Flash.Configuration.Common.Handlers.AssemblyParser;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Core;
using Moq;

namespace Flash.Configuration.Common.Tests.AssemblyParser;

public class AssemblyProcessorTests
{
    private readonly AssemblyProcessor _processor;

    public AssemblyProcessorTests()
    {
        Mock<IPropertyProcessor> mockPropertyProcessor = new();
        Mock<IFieldProcessor> mockFieldProcessor = new();
        _processor = new AssemblyProcessor(mockPropertyProcessor.Object, mockFieldProcessor.Object);
    }

    [Fact]
    public void ParseAssembly_NullPath_ThrowsArgumentException()
    {
        Assert.Throws<InvalidOperationException>(() => _processor.ParseAssembly(null!));
    }

    [Fact]
    public void ParseAssembly_EmptyPath_ThrowsArgumentException()
    {
        Assert.Throws<InvalidOperationException>(() => _processor.ParseAssembly(""));
    }

    [Fact]
    public void ParseAssembly_FileDoesNotExist_ReturnsEmptyConfig()
    {
        var result = _processor.ParseAssembly("non_existent.dll");
        Assert.Empty(result);
    }

    [Fact]
    public void ProcessType_TypeWithFlashConfigAttribute_AddsToConfig()
    {
        var result = _processor.ParseAssembly(typeof(ValidConfigClass).Assembly.Location);

        Assert.NotEmpty(result);
        Assert.Contains("development", result);
        Assert.Contains("ConfigName", result["development"]);
    }

    [FlashIgnore]
    private class NoAttributesClass
    {
        public string Property { get; set; } = "Test";
    }

    [FlashConfig(displayName:"ConfigName", environment: "Development")]
    private class ValidConfigClass
    {
        [FlashField(displayName:"FieldName")]
        public string ConfigField = "FieldValue";

        [FlashProperty(displayName: "PropertyName")]
        public string ConfigProperty { get; set; } = "PropertyValue";
    }
}