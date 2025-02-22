namespace Flash.Configuration.Common.Handlers.CommandLineParser.Abstraction;

public interface ICommandProcessor
{
    /// <summary>
    /// Parse command arguments in the application
    /// </summary>
    /// <param name="args"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> ParseArgsAsync<T>(string[] args) where T : class, new();
}