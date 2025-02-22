namespace Flash.Configuration.Updater.Abstraction;

public interface IAppHandler
{
    /// <summary>
    /// Process assembly reflection
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    Task HandleAsync(string[] args);
}