using Flash.Configuration.Common.Handlers.AssemblyParser;
using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Core;
using Moq;

namespace Flash.Configuration.Common.Tests.AssemblyParser;

public class PropertyProcessorTests
{
    private readonly PropertyProcessor _processor;
    private readonly Mock<ISectionProcessor> _mockSectionProcessor;
    private const string Environment = "None";

    public PropertyProcessorTests()
    {
        _mockSectionProcessor = new Mock<ISectionProcessor>();
        _processor = new PropertyProcessor(_mockSectionProcessor.Object);
    }
    
    [Fact]
    public void ProcessProperty_ShouldThrowArgumentNullException_WhenArgumentsAreNull()
    {
        Assert.Throws<ArgumentNullException>(() => _processor.ProcessProperty(null!, null!, null!, null!));
    }
    
    [Fact]
    public void ProcessProperty_ShouldIgnore_WhenNoFlashPropertyAttribute()
    {
        var property = typeof(TestClass).GetProperty(nameof(TestClass.NoAttributeProperty));
        var configValues = new Dictionary<string, object>();

        _processor.ProcessProperty(new TestClass(), property!, configValues, "None");

        Assert.Empty(configValues);
    }
    
    [Fact]
    public void ProcessProperty_ShouldIgnore_WhenFlashValueIgnoreAttributeExists()
    {
        var property = typeof(TestClass).GetProperty(nameof(TestClass.IgnoredProperty));
        var configValues = new Dictionary<string, object>();

        _processor.ProcessProperty(new TestClass(), property!, configValues, "None");

        Assert.Empty(configValues);
    }
    
    [Fact]
    public void ProcessProperty_ShouldIgnore_WhenFlashIgnorePropertyExists()
    {
        var property = typeof(TestClass).GetProperty(nameof(TestClass.IgnoredFullProperty));
        var configValues = new Dictionary<string, object>();

        _processor.ProcessProperty(new TestClass(), property!, configValues, "None");

        Assert.Empty(configValues);
    }

    [Fact]
    public void ProcessProperty_NullInstance_ThrowsArgumentNullException()
    {
        // Arrange
        var property = typeof(TestClass).GetProperty(nameof(TestClass.PropertyOne));
        var configValues = new Dictionary<string, object>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _processor.ProcessProperty(null!, property!, configValues, Environment));
    }

    [Fact]
    public void ProcessProperty_NullProperty_ThrowsArgumentNullException()
    {
        // Arrange
        var instance = new TestClass();
        var configValues = new Dictionary<string, object>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _processor.ProcessProperty(instance, null!, configValues, Environment));
    }

    [Fact]
    public void ProcessProperty_NullConfigValues_ThrowsArgumentNullException()
    {
        // Arrange
        var instance = new TestClass();
        var property = typeof(TestClass).GetProperty(nameof(TestClass.PropertyOne));

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _processor.ProcessProperty(instance, property!, null!, Environment));
    }

    [Fact]
    public void ProcessProperty_NoFlashPropertyAttribute_DoesNotModifyConfigValues()
    {
        // Arrange
        var instance = new TestClass();
        var property = typeof(TestClass).GetProperty(nameof(TestClass.NoAttributeProperty));
        var configValues = new Dictionary<string, object>();

        // Act
        _processor.ProcessProperty(instance, property!, configValues, Environment);

        // Assert
        Assert.Empty(configValues);
    }

    [Fact]
    public void ProcessProperty_SimpleProperty_AddsToConfigValues()
    {
        // Arrange
        var instance = new TestClass { PropertyOne = "TestValue" };
        var property = typeof(TestClass).GetProperty(nameof(TestClass.PropertyOne));
        var configValues = new Dictionary<string, object>();

        // Act
        _processor.ProcessProperty(instance, property!, configValues, Environment);

        // Assert
        Assert.Single(configValues);
        Assert.Equal("TestValue", configValues["CustomName1"]);
    }

    [Fact]
    public void ProcessProperty_NullValue_DoesNotModifyConfigValues()
    {
        // Arrange
        var instance = new TestClass { PropertyOne = null! };
        var property = typeof(TestClass).GetProperty(nameof(TestClass.PropertyOne));
        var configValues = new Dictionary<string, object>();

        // Act
        _processor.ProcessProperty(instance, property!, configValues, Environment);

        // Assert
        Assert.Empty(configValues);
    }

    [Fact]
    public void ProcessProperty_EnumWithFlashEnumString_ConvertsToString()
    {
        // Arrange
        var instance = new TestClass { EnumProperty0 = Tests.ValueTwo };
        var property = typeof(TestClass).GetProperty(nameof(TestClass.EnumProperty0));
        var configValues = new Dictionary<string, object>();

        // Act
        _processor.ProcessProperty(instance, property!, configValues, Environment);

        // Assert
        Assert.Single(configValues);
        Assert.Equal("ValueTwo", configValues["EnumProperty0"]);
    }

    [Fact]
    public void ProcessProperty_ComplexType_CallsSectionProcessor()
    {
        // Arrange
        var instance = new TestClass { ComplexProperty = new ComplexType { SomeValue = 123 } };
        var property = typeof(TestClass).GetProperty(nameof(TestClass.ComplexProperty));
        var configValues = new Dictionary<string, object>();
        var processedComplexData = new Dictionary<string, object> { { "SomeValue", 123 } };

        _mockSectionProcessor.Setup(sp => sp.ProcessComplexValue(instance.ComplexProperty, Environment))
            .Returns(processedComplexData);

        // Act
        _processor.ProcessProperty(instance, property!, configValues, Environment);

        // Assert
        Assert.Single(configValues);
        Assert.Equal(processedComplexData, configValues["ComplexProperty"]);
        _mockSectionProcessor.Verify(sp => sp.ProcessComplexValue(instance.ComplexProperty, Environment), Times.Once);
    }
    
    [Fact]
    public void ProcessDefaultProperty_ShouldAssignEnumAsString_WhenFlashEnumStringAttributeIsPresent()
    {
        // Arrange
        var configValues = new Dictionary<string, object>();
        var property = typeof(TestClass).GetProperty(nameof(TestClass.EnumProperty1))!;
        var attributeValue = new FlashValueAttribute(Tests.ValueTwo);

        // Act
        _processor.ProcessDefaultProperty(property, configValues, attributeValue, "EnumProperty");

        // Assert
        Assert.Equal("ValueTwo", configValues["EnumProperty"].ToString());
    }

    [Fact]
    public void ProcessDefaultProperty_ShouldAssignEnumAsValue_WhenNoFlashEnumStringAttribute()
    {
        // Arrange
        var configValues = new Dictionary<string, object>();
        var property = typeof(TestClass).GetProperty(nameof(TestClass.EnumWithoutString))!;
        var attributeValue = new FlashValueAttribute(Tests.ValueOne);

        // Act
        _processor.ProcessDefaultProperty(property, configValues, attributeValue, "EnumWithoutString");

        // Assert
        Assert.Equal(Tests.ValueOne.ToString(), configValues["EnumWithoutString"].ToString());
    }

    [Fact]
    public void ProcessDefaultProperty_ShouldAssignDefaultValue_ForRegularProperty()
    {
        // Arrange
        var configValues = new Dictionary<string, object>();
        var property = typeof(TestClass).GetProperty(nameof(TestClass.StringProperty))!;
        var attributeValue = new FlashValueAttribute("TestValue");

        // Act
        _processor.ProcessDefaultProperty(property, configValues, attributeValue, "StringProperty");

        // Assert
        Assert.Equal("TestValue", configValues["StringProperty"]);
    }

    [Fact]
    public void ProcessExtendedProperty_SetsSimpleCollection_WhenPropertyIsNotComplex()
    {
        var property = typeof(TestClass).GetProperty(nameof(TestClass.SimpleCollectionProperty))!;
        var configValues = new Dictionary<string, object>();
        var attribute = new FlashPropertyAttribute();
        var values = new List<object> { "Value1", "Value2" };
        
        _processor.ProcessExtendedProperty(property, configValues, "testEnv", values, attribute, "SimpleProp");
        
        Assert.Equal(new List<object> { "Value1", "Value2" }, configValues["SimpleProp"]);
    }

    private class TestClass
    {
        [FlashProperty(displayName: "CustomName1")]
        public string PropertyOne { get; set; } = "prop1";

        public string NoAttributeProperty { get; set; } = "not attribute";
        
        [FlashIgnore]
        public string IgnoredFullProperty { get; set; } = "invisible property";

        [FlashProperty]
        public Tests EnumProperty0 { get; set; } = Tests.ValueTwo;
        
        [FlashEnum]
        public Tests EnumProperty1 { get; set; }

        public Tests EnumWithoutString { get; set; } = Tests.ValueOne;
        
        public string StringProperty { get; set; } = string.Empty;

        [FlashProperty(isComplex: true)]
        public ComplexType ComplexProperty { get; set; } = new ComplexType() { SomeValue = 3 };
        
        [FlashProperty(displayName: "IgnoredProp")]
        [FlashValueIgnore(environment: "None")]
        public string IgnoredProperty { get; set; } = "Ignored";
        public List<object> SimpleCollectionProperty { get; set; } = [];
    }

    [FlashSection]
    private class ComplexType
    {
        [FlashProperty] public int SomeValue { get; set; }
    }

    [FlashEnumString]
    private enum Tests
    {
        ValueOne,
        ValueTwo
    }
}