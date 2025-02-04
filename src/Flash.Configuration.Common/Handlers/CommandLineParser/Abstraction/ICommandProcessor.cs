namespace Flash.Configuration.Common.Handlers.CommandLineParser.Abstraction;

public interface ICommandProcessor
{
    Task<T> ParseArgsAsync<T>(string[] args) where T : class, new();
}