using Flash.Configuration.Common.Models;
using Flash.Configuration.Updater;
namespace Flash.Configuration.Updater.Tests;

public class AppValidatorTests
{
    private readonly AppValidator _validator = new();

    [Fact]
    public void Validate_NullOptions_ThrowsArgumentNullException()
    {
        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => _validator.Validate(null!));
        Assert.Contains("Command line arguments", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid\\path")]
    [InlineData(null)]
    public void Validate_InvalidProjectPath_ThrowsArgumentException(string? projectPath)
    {
        // Arrange
        var options = new ArgsOptions { ProjectPath = projectPath!, OutputPath = "bin", AssemblyName = "app.dll" };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => _validator.Validate(options));
        Assert.Contains("Project path", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_MissingOutputPath_ThrowsArgumentException(string? outputPath)
    {
        // Arrange
        var options = new ArgsOptions { ProjectPath = @"C:\ValidPath", OutputPath = outputPath!, AssemblyName = "app.dll" };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => _validator.Validate(options));
        Assert.Contains("Output path", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid.exe")]
    [InlineData("no_extension")]
    [InlineData(null)]
    public void Validate_InvalidAssemblyName_ThrowsArgumentException(string? assemblyName)
    {
        // Arrange
        var options = new ArgsOptions { ProjectPath = @"C:\ValidPath", OutputPath = "bin", AssemblyName = assemblyName! };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => _validator.Validate(options));
        Assert.Contains("Assembly name", ex.Message);
    }

    [Fact]
    public void Validate_ValidArguments_DoesNotThrow()
    {
        // Arrange
        var options = new ArgsOptions { ProjectPath = @"C:\VirtualPath", OutputPath = "bin\\Debug\\net8.0", AssemblyName = "Flash.Configuration.TestConsole.dll" };

        // Act & Assert
        _validator.Validate(options);
    }
}