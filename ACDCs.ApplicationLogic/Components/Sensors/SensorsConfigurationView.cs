namespace ACDCs.API.Core.Components.Sensors;

using System.Collections.ObjectModel;
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
            .BackgroundColor(API.Instance.Foreground.WithAlpha(0.7f))
            .Row(1)
            .Column(0)
            .Margin(2);
        //    .ItemTemplate(new DataTemplate(typeof(UsedSensorsViewCell)));
        Add(_usedSensorsCollectionView);

        _availableSensorsCollectionView = new CollectionView()
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

    private static void GetAvailable(Dictionary<Type, bool> sensors, string location, ObservableCollection<SensorItem> items)
    {
        foreach (KeyValuePair<Type, bool> avail in sensors)
        {
            if (!avail.Value)
            {
                continue;
            }

            SensorItem? item = new SensorItem(
                avail.Key.Name,
                location,
                avail.Key.Name,
                SensorSpeed.Fastest,
                avail.Key.Name,
                avail.Key.Name
            );

            items.Add(
                item
            );
        }
    }

    private void AddToUsedClicked(object? sender, EventArgs e)
    {
    }

    private async void NewRemoteClicked(object? sender, EventArgs e)
    {
        if (Uri.TryCreate(_addNewRemoteEntry.Text, UriKind.Absolute, out Uri? baseUrl))
        {
            Dictionary<Type, bool>? availability = await DownloadClient.GetSensorAvailability(baseUrl);
            ObservableCollection<SensorItem>? items =
                _availableSensorsCollectionView.ItemsSource as ObservableCollection<SensorItem>;
            if (availability == null || items == null)
            {
                return;
            }

            GetAvailable(availability, baseUrl.ToString(), items);
        }
    }

    private void OnLoad(object? sender, EventArgs e)
    {
        ObservableCollection<SensorItem> items = new();
        Dictionary<Type, bool> sensors = SensorAvailability.GetAvailableSensors();
        string location = "local";
        GetAvailable(sensors, location, items);
        _availableSensorsCollectionView.ItemsSource = items;
    }

    private void RemoveFromUsedClicked(object? sender, EventArgs e)
    {
    }
}
