namespace ACDCs.API.Core.Components.Sensors;

using System.Collections.ObjectModel;
using ACDCs.IO.DB;
using ACDCs.Sensors.API;
using ACDCs.Sensors.API.Client;
using ACDCs.Sensors.API.Sensors;
using Instance;

public class SensorsConfigurationView : Grid
{
    private readonly Button _addNewRemoteButton;
    private readonly Entry _addNewRemoteEntry;
    private readonly Button _addToUsedButton;
    private readonly CollectionView _availableSensorsCollectionView;
    private readonly Button _removeFromUsedButton;
    private readonly CollectionView _usedSensorsCollectionView;

    public SensorsConfigurationView()
    {
        RowDefinition[] rows =
        {
            new(34) ,
            new(GridLength.Star),
            new(40)
        };

        ColumnDefinition[] columns =
        {
            new(GridLength.Star),
            new(64),
            new(GridLength.Star)
        };

        this.RowDefinitions(rows)
            .ColumnDefinitions(columns)
            .ColumnSpacing(0)
            .RowSpacing(0)
            .Padding(0)
            .Margin(2);

        _usedSensorsCollectionView = new CollectionView()
            .SelectionMode(SelectionMode.Single)
            .BackgroundColor(API.Instance.Foreground.WithAlpha(0.7f))
            .Row(1)
            .Column(0)
            .Margin(2)
          .ItemTemplate(new SensorsDataTemplate());
        Add(_usedSensorsCollectionView);

        _availableSensorsCollectionView = new CollectionView()
            .SelectionMode(SelectionMode.Single)
            .BackgroundColor(API.Instance.Foreground.WithAlpha(0.7f))
            .Row(1)
            .Column(2)
            .Margin(2)
          .ItemTemplate(new SensorsDataTemplate());
        Add(_availableSensorsCollectionView);

        _addNewRemoteEntry = new Entry("URL of Server")
            .WidthRequest(100);

        _addNewRemoteButton = new Button("Add remote sensors")
            .OnClicked(NewRemoteClicked);

        Add(new HorizontalStackLayout
        {
            _addNewRemoteEntry, _addNewRemoteButton
        }.Column(2));

        _addToUsedButton = new Button("Add")
            .WidthRequest(60)
            .OnClicked(AddToUsedClicked);
        _removeFromUsedButton = new Button("Remove")
            .WidthRequest(60)
            .OnClicked(RemoveFromUsedClicked);

        Add(new VerticalStackLayout
            {
                _addToUsedButton,
                _removeFromUsedButton
            }
            .Row(1)
            .Column(1)
            .VerticalOptions(LayoutOptions.Center));

        this.OnLoaded(OnLoad);
    }

    public static List<SensorItem> GetSavedSensors()
    {
        DBConnection usedSensorsDb = new("Sensors");

        List<SensorItem> sensorItems = usedSensorsDb.Read<SensorItem>("UsedSensors");
        return sensorItems;
    }

    private void AddToUsedClicked(object? sender, EventArgs e)
    {
        if (_availableSensorsCollectionView.SelectedItem is not SensorItem item) return;
        if (_availableSensorsCollectionView.ItemsSource is not ObservableCollection<SensorItem> items) return;
        if (_usedSensorsCollectionView.ItemsSource is not ObservableCollection<SensorItem> itemsUsed) return;
        items.Remove(item);
        itemsUsed.Add(item);
        SaveSensors(itemsUsed);
    }

    private async void NewRemoteClicked(object? sender, EventArgs e)
    {
        if (Uri.TryCreate(_addNewRemoteEntry.Text, UriKind.Absolute, out Uri? baseUrl))
        {
            List<SensorItem>? sensors = await DownloadClient.GetSensorAvailability(baseUrl);
            if (sensors == null || _availableSensorsCollectionView.ItemsSource is not ObservableCollection<SensorItem> items)
            {
                return;
            }

            foreach (SensorItem item in sensors)
            {
                item.Location = $"{baseUrl.ToString().TrimEnd('/')}{item.Location}";
                items.Add(item);
            }
        }
    }

    private void OnLoad(object? sender, EventArgs e)
    {
        ObservableCollection<SensorItem> items = new();
        ObservableCollection<SensorItem> itemsUsed = new(GetSavedSensors());
        _usedSensorsCollectionView.ItemsSource = itemsUsed;
        List<SensorItem> sensors = SensorAvailability.GetAvailableSensors();

        string location = "local";
        foreach (SensorItem item in sensors)
        {
            item.Location = location;
            items.Add(item);
        }
        _availableSensorsCollectionView.ItemsSource = items;
    }

    private void RemoveFromUsedClicked(object? sender, EventArgs e)
    {
        if (_usedSensorsCollectionView.SelectedItem is not SensorItem item) return;
        if (_availableSensorsCollectionView.ItemsSource is not ObservableCollection<SensorItem> items) return;
        if (_usedSensorsCollectionView.ItemsSource is not ObservableCollection<SensorItem> itemsUsed) return;
        items.Add(item);
        itemsUsed.Remove(item);
        SaveSensors(itemsUsed);
    }

    private void SaveSensors(ObservableCollection<SensorItem> itemsUsed)
    {
        DBConnection usedSensorsDb = new("Sensors");
        usedSensorsDb.ReWrite(itemsUsed.ToList(), "UsedSensors");
    }
}
