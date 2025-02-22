using Flash.Configuration.Common.Handlers.AssemblyParser;
using Flash.Configuration.Core;

namespace Flash.Configuration.Common.Tests.AssemblyParser;

public class SectionProcessorTests
{
    private readonly SectionProcessor _processor = new();
    private const string Environment = "None";

    [Fact]
    public void ProcessComplexValue_NullInput_ThrowsArgumentNullException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => _processor.ProcessComplexValue(null!, Environment));
        Assert.StartsWith("Value", ex.Message);
    }

    [Fact]
    public void ProcessComplexValue_NoFlashPropertyAttributes_ReturnsEmptyDictionary()
    {
        // Arrange
        var obj = new { Name = "Test", Age = 30 };

        // Act
        var result = _processor.ProcessComplexValue(obj, Environment);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ProcessComplexValue_WithFlashPropertyAttributes_ReturnsValidDictionary()
    {
        // Arrange
        var obj = new TestClass
        {
            PropertyOne = "Value1",
            PropertyTwo = 42,
            IgnoredProperty = "Ignored"
        };

        // Act
        var result = _processor.ProcessComplexValue(obj, Environment);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains("CustomName1", result);
        Assert.Contains("PropertyTwo", result);
        Assert.Equal("Value1", result["CustomName1"]);
        Assert.Equal(42, result["PropertyTwo"]);
    }

    [Fact]
    public void ProcessComplexValue_NullProperty_IgnoresIt()
    {
        // Arrange
        var obj = new TestClass { PropertyOne = null!, PropertyTwo = 42 };

        // Act
        var result = _processor.ProcessComplexValue(obj, Environment);

        // Assert
        Assert.Single(result);
        Assert.Contains("PropertyTwo", result);
        Assert.Equal(42, result["PropertyTwo"]);
    }

    private class TestClass
    {
        [FlashProperty(displayName: "CustomName1")]
        public string PropertyOne { get; set; } = "prop1";

        [FlashProperty]
        public int PropertyTwo { get; set; } = 2;

        [FlashIgnore] public string IgnoredProperty { get; set; } = "prop2";
    }
}