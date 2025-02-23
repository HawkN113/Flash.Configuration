namespace Flash.Configuration.Common.Providers.Abstraction;

public interface IFileProvider
{
    bool Exists(string? path);
    Task<string> ReadAllTextAsync(string path);
    Task WriteAllTextAsync(string path, string? contents);
}