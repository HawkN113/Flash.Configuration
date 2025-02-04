namespace Flash.Configuration.Abstraction;

internal interface IAppHandler
{
    Task HandleAsync(string[] args);
}