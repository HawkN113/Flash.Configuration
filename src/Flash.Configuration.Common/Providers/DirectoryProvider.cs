using Flash.Configuration.Common.Providers.Abstraction;

namespace Flash.Configuration.Common.Providers;

internal sealed class DirectoryProvider : IDirectoryProvider
{
    public bool Exists(string path) => Directory.Exists(path);

    public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption) =>
        Directory.EnumerateFiles(path, searchPattern, searchOption);
}