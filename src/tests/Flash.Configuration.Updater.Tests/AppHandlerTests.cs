using Flash.Configuration.Common.Handlers.AssemblyParser.Abstraction;
using Flash.Configuration.Common.Handlers.CommandLineParser.Abstraction;
using Flash.Configuration.Common.Handlers.ConfigParser.Abstraction;
using Flash.Configuration.Common.Models;
using Flash.Configuration.Updater.Abstraction;
using Moq;
namespace Flash.Configuration.Updater.Tests;

public class AppHandlerTests
{
    private readonly Mock<IAppValidator> _mockAppValidator = new();
    private readonly Mock<IConfigProcessor> _mockConfigProcessor = new();
    private readonly Mock<ICommandProcessor> _mockCommandProcessor = new();
    private readonly Mock<IAssemblyProcessor> _mockAssemblyProcessor = new();

    [Fact]
    public async Task HandleAsync_ShouldCallValidate_WhenArgumentsAreParsed()
    {
        // Arrange
        var args = new[] { "some", "arguments" };
        var options = new ArgsOptions();
        _mockCommandProcessor.Setup(cp => cp.ParseArgsAsync<ArgsOptions>(args)).ReturnsAsync(options);

        var handler = new AppHandler(
            _mockAppValidator.Object,
            _mockConfigProcessor.Object,
            _mockCommandProcessor.Object,
            _mockAssemblyProcessor.Object
        );
            
        await using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        Console.SetError(consoleOutput);

        // Act
        await handler.HandleAsync(args);

        // Assert
        _mockAppValidator.Verify(v => v.Validate(options), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateConfigs_WhenConfigsExist()
    {
        // Arrange
        var args = new[] { "some", "arguments" };
        var options = new ArgsOptions();
        var configData = new Dictionary<string, Dictionary<string, object>>()
        {
            { "none", new Dictionary<string, object> { { "Key", "Value" } } }
        };
        var projectConfigs = new[] { new ConfigDetails { FilePath = "appsettings.json" } };
        var outputConfigs = new[] { new ConfigDetails { FilePath = "appsettings.json" } };

        _mockCommandProcessor.Setup(cp => cp.ParseArgsAsync<ArgsOptions>(args)).ReturnsAsync(options);
        _mockAssemblyProcessor.Setup(ap => ap.ParseAssembly(It.IsAny<string>())).Returns(configData);
        _mockConfigProcessor.Setup(cp => cp.GetAvailableConfigs(It.IsAny<string>())).Returns(projectConfigs);
        _mockConfigProcessor.Setup(cp => cp.GetAvailableConfigs(It.IsAny<string>())).Returns(outputConfigs);
        _mockConfigProcessor.Setup(cp =>
                cp.UpdateConfigAsync(It.IsAny<ConfigDetails>(), It.IsAny<Dictionary<string, object>>()))
            .ReturnsAsync(Result.Success());

        var handler = new AppHandler(
            _mockAppValidator.Object,
            _mockConfigProcessor.Object,
            _mockCommandProcessor.Object,
            _mockAssemblyProcessor.Object
        );

        // Act
        await handler.HandleAsync(args);

        // Assert
        _mockConfigProcessor.Verify(cp => cp.UpdateConfigAsync(It.IsAny<ConfigDetails>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
    }

    [Fact]
    public async Task HandleAsync_ShouldWriteWarning_WhenNoConfigsAreFound()
    {
        // Arrange
        var args = new[] { "some", "arguments" };
        var options = new ArgsOptions();
        _mockCommandProcessor.Setup(cp => cp.ParseArgsAsync<ArgsOptions>(args)).ReturnsAsync(options);

        var configData = new Dictionary<string, Dictionary<string, object>>();
        var projectConfigs = Array.Empty<ConfigDetails>();
        var outputConfigs = Array.Empty<ConfigDetails>();

        _mockAssemblyProcessor.Setup(ap => ap.ParseAssembly(It.IsAny<string>())).Returns(configData);
        _mockConfigProcessor.Setup(cp => cp.GetAvailableConfigs(It.IsAny<string>())).Returns(projectConfigs);
        _mockConfigProcessor.Setup(cp => cp.GetAvailableConfigs(It.IsAny<string>())).Returns(outputConfigs);

        var handler = new AppHandler(
            _mockAppValidator.Object,
            _mockConfigProcessor.Object,
            _mockCommandProcessor.Object,
            _mockAssemblyProcessor.Object
        );

        await using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        Console.SetError(consoleOutput);

        // Act
        await handler.HandleAsync(args);

        // Assert
        Assert.Contains("WARNING: No configuration files or data found.", consoleOutput.ToString());
    }

    [Fact]
    public async Task HandleAsync_ShouldHandleException_WhenThrown()
    {
        // Arrange
        var args = new string[] { "some", "arguments" };
        _mockCommandProcessor.Setup(cp => cp.ParseArgsAsync<ArgsOptions>(args)).ThrowsAsync(new Exception("Test Exception"));

        var handler = new AppHandler(
            _mockAppValidator.Object,
            _mockConfigProcessor.Object,
            _mockCommandProcessor.Object,
            _mockAssemblyProcessor.Object
        );

        await using var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act
        await handler.HandleAsync(args);

        // Assert
        Assert.Contains("ERROR: Test Exception", consoleOutput.ToString());
    }
}