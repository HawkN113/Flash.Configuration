using Flash.Configuration.Common.Handlers.CommandLineParser;
using Flash.Configuration.Common.Handlers.CommandLineParser.Attributes;

namespace Flash.Configuration.Common.Tests.CommandLineParser;

public class CommandProcessorTests
{
    private readonly CommandProcessor _processor = new();

    private class TestOptions
    {
        public string Url { get; set; } = null!;
        public int Port { get; set; } = 0;
        public bool Debug { get; set; } = false;

        [PropertyAlias("user")] public string Username { get; set; } = null!;
    }

    [Fact]
    public async Task ParseArgsAsync_ShouldParseSimpleArguments()
    {
        // Arrange
        var args = new[] { "--Url=https://example.com", "--Port=8080", "--Debug=true" };

        // Act
        var result = await _processor.ParseArgsAsync<TestOptions>(args);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("https://example.com", result.Url);
        Assert.Equal(8080, result.Port);
        Assert.True(result.Debug);
    }

    [Fact]
    public async Task ParseArgsAsync_ShouldHandleBooleanFlags()
    {
        // Arrange
        var args = new[] { "--Debug" }; // Boolean без значения

        // Act
        var result = await _processor.ParseArgsAsync<TestOptions>(args);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Debug);
    }

    [Fact]
    public async Task ParseArgsAsync_ShouldUsePropertyAlias()
    {
        // Arrange
        var args = new[] { "--user=admin" };

        // Act
        var result = await _processor.ParseArgsAsync<TestOptions>(args);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("admin", result.Username);
    }

    [Fact]
    public async Task ParseArgsAsync_ShouldIgnoreUnknownArguments()
    {
        // Arrange
        var args = new[] { "--Unknown=xyz", "--Port=9090" };

        // Act
        var result = await _processor.ParseArgsAsync<TestOptions>(args);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(9090, result.Port);
        Assert.Null(result.Url);
        Assert.Null(result.Username);
    }

    [Fact]
    public async Task ParseArgsAsync_ShouldHandleEmptyArguments()
    {
        // Arrange
        var args = Array.Empty<string>();

        // Act
        var result = await _processor.ParseArgsAsync<TestOptions>(args);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Url);
        Assert.Equal(0, result.Port);
        Assert.False(result.Debug);
    }
}