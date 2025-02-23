using Flash.Configuration.Common.Handlers.AssemblyParser;
using Flash.Configuration.Core;

namespace Flash.Configuration.Common.Tests.AssemblyParser;

public class FieldProcessorTests
{
    private readonly FieldProcessor _processor = new();
    private const string Environment = "None";

    [Fact]
    public void ProcessField_NullInstance_ThrowsArgumentNullException()
    {
        // Arrange
        var field = typeof(TestClass).GetField(nameof(TestClass.FieldOne));
        var configValues = new Dictionary<string, object>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _processor.ProcessField(null!, field!, configValues, Environment));
    }

    [Fact]
    public void ProcessField_NullField_ThrowsArgumentNullException()
    {
        // Arrange
        var instance = new TestClass();
        var configValues = new Dictionary<string, object>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _processor.ProcessField(instance, null!, configValues, Environment));
    }

    [Fact]
    public void ProcessField_NullConfigValues_ThrowsArgumentNullException()
    {
        // Arrange
        var instance = new TestClass();
        var field = typeof(TestClass).GetField(nameof(TestClass.FieldOne));

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _processor.ProcessField(instance, field!, null!, Environment));
    }

    [Fact]
    public void ProcessField_NoFlashFieldAttribute_DoesNotModifyConfigValues()
    {
        // Arrange
        var instance = new TestClass();
        var field = typeof(TestClass).GetField(nameof(TestClass.NoAttributeField));
        var configValues = new Dictionary<string, object>();

        // Act
        _processor.ProcessField(instance, field!, configValues, Environment);

        // Assert
        Assert.Empty(configValues);
    }

    [Fact]
    public void ProcessField_SimpleField_AddsToConfigValues()
    {
        // Arrange
        var instance = new TestClass { FieldOne = "TestValue" };
        var field = typeof(TestClass).GetField(nameof(TestClass.FieldOne));
        var configValues = new Dictionary<string, object>();

        // Act
        _processor.ProcessField(instance, field!, configValues, Environment);

        // Assert
        Assert.Single(configValues);
        Assert.Equal("TestValue", configValues["CustomName1"]);
    }

    [Fact]
    public void ProcessField_NullValue_DoesNotModifyConfigValues()
    {
        // Arrange
        var instance = new TestClass { FieldOne = null! };
        var field = typeof(TestClass).GetField(nameof(TestClass.FieldOne));
        var configValues = new Dictionary<string, object>();

        // Act
        _processor.ProcessField(instance, field!, configValues, Environment);

        // Assert
        Assert.Empty(configValues);
    }

    private class TestClass
    {
        [FlashField(displayName: "CustomName1")]
        public string FieldOne = "field1";

        [FlashIgnore] public readonly string NoAttributeField = "test";

    }
}