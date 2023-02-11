namespace ACDCs.ApplicationLogic.Components.Items;

internal class FakeLocalFile : IDisposable
{
    public string FilePath { get; }

    /// <summary>
    /// Currently ImageSource.FromStream is not working on windows devices.
    /// This class saves the passed stream in a cache directory, returns the local path and deletes it on dispose.
    /// </summary>
    public FakeLocalFile(Stream source, string idFilePath)
    {
        FilePath = System.IO.Path.Combine(FileSystem.Current.CacheDirectory, $"{idFilePath}");
        using FileStream fs = new(FilePath, FileMode.Create);
        source.CopyTo(fs);
    }

    public void Dispose()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}
