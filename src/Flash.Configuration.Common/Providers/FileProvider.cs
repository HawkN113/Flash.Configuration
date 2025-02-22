using Flash.Configuration.Common.Providers.Abstraction;

namespace Flash.Configuration.Common.Providers;

internal sealed class FileProvider : IFileProvider
{
    public bool Exists(string? path)
    {
        return File.Exists(path);
    }

    public async Task<string> ReadAllTextAsync(string path)
    {
        return await File.ReadAllTextAsync(path);
    }

    public async Task WriteAllTextAsync(string path, string? contents)
    {
        await File.WriteAllTextAsync(path, contents);
    }
}