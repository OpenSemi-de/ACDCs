namespace ACDCs.Sensors.Client;

using CommunityToolkit.Maui.Storage;

public partial class MainPage : ContentPage
{
    private DownloadWorker? _downloadWorker;
    private string? _outputPath;
    private Uri? _uri;

    public MainPage()
    {
        InitializeComponent();
        Loaded += MainPage_Loaded;
        Unloaded += MainPage_Unloaded;
    }

    private void MainPage_Loaded(object? sender, EventArgs e)
    {
        urlEntry.Text = "http://192.168.178.30:5000";
    }

    private void MainPage_Unloaded(object? sender, EventArgs e)
    {
        _downloadWorker?.Stop();
    }

    private async void PickDirectory_Clicked(object sender, EventArgs e)
    {
        try
        {
            var folder = await FolderPicker.Default.PickAsync(CancellationToken.None);
            _outputPath = folder.Path;
            DirectoryEntry.Text = _outputPath;
        }
        catch
        {
            // ignored
        }
    }

    private async void StartClient_Clicked(object sender, EventArgs e)
    {
        _downloadWorker = new DownloadWorker(_outputPath, _uri);
        await _downloadWorker.Start();
    }

    private void StopClient_Clicked(object sender, EventArgs e)
    {
        _downloadWorker?.Stop();
    }

    private void UrlEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        urlEntry.BackgroundColor = Uri.TryCreate(e.NewTextValue, UriKind.Absolute, out _uri) ? Colors.Green : Colors.Red;
    }
}
