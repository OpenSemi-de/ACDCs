namespace ACDCs.Sensors.Client;

using API.Client;
using CommunityToolkit.Maui.Storage;
using Sharp.UI;

public class MainPage : ContentPage
{
    private readonly StackLayout _mainLayout;
    private DownloadWorker? _downloadWorker;
    private string? _outputPath;
    private Entry? _targetDirectoryEntry;
    private Uri? _uri;
    private Entry? _urlEntry;

    public MainPage()
    {
        Loaded += MainPage_Loaded;
        Unloaded += MainPage_Unloaded;
        _mainLayout = new StackLayout()
            .Orientation(StackOrientation.Vertical);

        InitializeComponent();

        Content = _mainLayout;
    }

    private void InitializeComponent()
    {
        _urlEntry = new Entry("url")
            .OnTextChanged(UrlEntry_TextChanged)
            .WidthRequest(400);
        _mainLayout.Add(_urlEntry);

        _targetDirectoryEntry = new Entry()
            .WidthRequest(400);

        _mainLayout.Add(new VerticalStackLayout
        {
            _targetDirectoryEntry,
            new Button("Select directory")
                .OnClicked(PickDirectory_Clicked)
        });

        _mainLayout.Add(new VerticalStackLayout
        {
            new Button("Start")
                .OnClicked(StartClient_Clicked),
            new Button("Stop")
                .OnClicked(StopClient_Clicked)
        });
    }

    private void MainPage_Loaded(object? sender, EventArgs e)
    {
        if (_urlEntry != null)
        {
            _urlEntry.Text = "http://192.168.178.30:5000";
        }
    }

    private void MainPage_Unloaded(object? sender, EventArgs e)
    {
        _downloadWorker?.Stop();
    }

    private async void PickDirectory_Clicked(object? sender, EventArgs e)
    {
        try
        {
            var folder = await FolderPicker.Default.PickAsync(CancellationToken.None);
            _outputPath = folder.Folder?.Path;
            if (_targetDirectoryEntry != null)
            {
                _targetDirectoryEntry.Text = _outputPath;
            }
        }
        catch
        {
            // ignored
        }
    }

    private async void StartClient_Clicked(object? sender, EventArgs e)
    {
        _downloadWorker = new DownloadWorker(_outputPath, _uri);
        await _downloadWorker.Start();
    }

    private void StopClient_Clicked(object? sender, EventArgs e)
    {
        _downloadWorker?.Stop();
    }

    private void UrlEntry_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_urlEntry != null)
        {
            _urlEntry.BackgroundColor =
                Uri.TryCreate(e.NewTextValue, UriKind.Absolute, out _uri) ? Colors.Green : Colors.Red;
        }
    }
}
