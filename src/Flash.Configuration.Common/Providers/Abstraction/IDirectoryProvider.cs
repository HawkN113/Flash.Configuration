namespace Flash.Configuration.Common.Providers.Abstraction;

public interface IDirectoryProvider
{
    bool Exists(string path);
    IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);
}