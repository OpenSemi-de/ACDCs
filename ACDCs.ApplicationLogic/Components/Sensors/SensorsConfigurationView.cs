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
    private readonly ListView _availableSensorsListView;
    private readonly Button _removeFromUsedButton;
    private readonly ListView _usedSensorsListView;

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

        _usedSensorsListView = new ListView()
            .BackgroundColor(API.Instance.Foreground.WithAlpha(0.7f))
            .Row(1)
            .Column(0)
            .Margin(2)
            .ItemTemplate(new DataTemplate(typeof(UsedSensorsViewCell)));
        Add(_usedSensorsListView);

        _availableSensorsListView = new ListView()
            .BackgroundColor(API.Instance.Foreground.WithAlpha(0.7f))
            .Row(1)
            .Column(2)
            .Margin(2)
            .ItemTemplate(new DataTemplate(typeof(AvailableSensorsViewCell)));
        Add(_availableSensorsListView);

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

    private void AddToUsedClicked(object? sender, EventArgs e)
    {
    }

    private void NewRemoteClicked(object? sender, EventArgs e)
    {
        if (Uri.TryCreate(_addNewRemoteEntry.Text, UriKind.Absolute, out Uri? baseUrl))
        {
            Task<Dictionary<Type, bool>?> availability = DownloadClient.GetSensorAvailability(baseUrl);
        }
    }

    private void OnLoad(object? sender, EventArgs e)
    {
        ObservableCollection<SensorItem> items = new();
        foreach (var avail in SensorAvailability.GetAvailableSensors())
        {
            if (!avail.Value)
            {
                continue;
            }

            SensorItem? item = new SensorItem(
                avail.Key.Name,
                "local",
                avail.Key.Name,
                SensorSpeed.Fastest,
                avail.Key.Name,
                avail.Key.Name
            );

            items.Add(
                item
            );
        }
        _availableSensorsListView.ItemsSource = items;
    }

    private void RemoveFromUsedClicked(object? sender, EventArgs e)
    {
    }
}
