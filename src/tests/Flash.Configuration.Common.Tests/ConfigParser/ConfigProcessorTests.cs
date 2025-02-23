using Flash.Configuration.Common.Handlers.ConfigParser;
using Flash.Configuration.Common.Models;
using Flash.Configuration.Common.Providers.Abstraction;
using Moq;

namespace Flash.Configuration.Common.Tests.ConfigParser;

public class ConfigProcessorTests
{
    private readonly Mock<IDirectoryProvider> _mockDirectoryProvider;
    private readonly Mock<IFileProvider> _mockFileProvider;
    private readonly ConfigProcessor _configProcessor;

    public ConfigProcessorTests()
    {
        _mockDirectoryProvider = new Mock<IDirectoryProvider>();
        _mockFileProvider = new Mock<IFileProvider>();
        _configProcessor = new ConfigProcessor(_mockDirectoryProvider.Object, _mockFileProvider.Object);
    }

    [Fact]
    public void GetAvailableConfigs_ReturnsEmpty_WhenDirectoryDoesNotExist()
    {
        // Arrange
        string directoryPath = "testPath";
        _mockDirectoryProvider.Setup(d => d.Exists(directoryPath)).Returns(false);

        // Act
        var result = _configProcessor.GetAvailableConfigs(directoryPath);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetAvailableConfigs_ReturnsFilteredFiles_WhenDirectoryExists()
    {
        // Arrange
        var directoryPath = "testPath";
        var validFile1 = "appsettings.json";
        var validFile2 = "appsettings.Development.json";
        var invalidFile = "invalid.txt";

        _mockDirectoryProvider.Setup(d => d.Exists(directoryPath)).Returns(true);
        _mockDirectoryProvider.Setup(d => d.EnumerateFiles(directoryPath, "*.json", SearchOption.TopDirectoryOnly))
            .Returns([validFile1, validFile2, invalidFile]);

        // Act
        var result = _configProcessor.GetAvailableConfigs(directoryPath);

        // Assert
        Assert.NotEmpty(result);
        Assert.All(result, config => Assert.EndsWith(".json", config.FilePath));
    }

    [Fact]
    public async Task UpdateConfigAsync_ReturnsFail_WhenFileDoesNotExist()
    {
        // Arrange
        var configDetails = new ConfigDetails { FilePath = "config.json" };
        _mockFileProvider.Setup(f => f.Exists(configDetails.FilePath)).Returns(false);

        // Act
        var result = await _configProcessor.UpdateConfigAsync(configDetails, new Dictionary<string, object>());

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal($"Config file '{configDetails.FilePath}' not found.", result.ErrorMessages);
    }

    [Fact]
    public async Task UpdateConfigAsync_ReturnsFail_WhenConfigDataIsNull()
    {
        // Arrange
        var configDetails = new ConfigDetails { FilePath = "config.json" };
        _mockFileProvider.Setup(f => f.Exists(configDetails.FilePath)).Returns(true);

        // Act
        var result = await _configProcessor.UpdateConfigAsync(configDetails, null);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("No configuration data provided.", result.ErrorMessages);
    }

    [Fact]
    public async Task UpdateConfigAsync_UpdatesFile_WhenValidDataProvided()
    {
        // Arrange
        var configDetails = new ConfigDetails { FilePath = "config.json" };
        var configData = new Dictionary<string, object>
        {
            { "Section1", new Dictionary<string, object> { { "Key1", "Value1" } } }
        };

        _mockFileProvider.Setup(f => f.Exists(configDetails.FilePath)).Returns(true);
        _mockFileProvider.Setup(f => f.ReadAllTextAsync(configDetails.FilePath)).ReturnsAsync("{}");

        // Act
        var result = await _configProcessor.UpdateConfigAsync(configDetails, configData);

        // Assert
        Assert.True(result.IsSuccessful);
        _mockFileProvider.Verify(f => f.WriteAllTextAsync(configDetails.FilePath, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateConfigAsync_HandlesException_AndReturnsFail()
    {
        // Arrange
        var configDetails = new ConfigDetails { FilePath = "config.json" };
        var configData = new Dictionary<string, object> { { "Key", "Value" } };

        _mockFileProvider.Setup(f => f.Exists(configDetails.FilePath)).Returns(true);
        _mockFileProvider.Setup(f => f.ReadAllTextAsync(configDetails.FilePath)).ThrowsAsync(new IOException("Read error"));

        // Act
        var result = await _configProcessor.UpdateConfigAsync(configDetails, configData);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Contains("Exception while updating", result.ErrorMessages);
    }
}