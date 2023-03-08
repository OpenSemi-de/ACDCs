namespace ACDCs.API.Core.Components.Sensors;

using System.Collections.ObjectModel;
using ACDCs.Sensors.API;
using ACDCs.Sensors.API.Client;
using ACDCs.Sensors.API.Interfaces;
using ACDCs.Sensors.API.Samples;
using ACDCs.Sensors.API.Sensors;
using Instance;
using Sharp.UI;

public class SensorsDisplayView : Grid
{
    private readonly Dictionary<SensorItem, DownloadClient> _clients = new();
    private readonly Label _selectedSensorLabel;
    private readonly CollectionView _usedSensorsCollectionView;
    private DownloadClient? _selectedClient;
    private readonly Timer _updateTimer;
    private readonly SensorValueDisplay _valueDisplay;

    private void UpdateGui(object? state)
    {
        if (_selectedClient?.SampleCache != null)
        {
            {
                if (_selectedClient != null)
                {
                    if (_selectedClient?.SampleCache != null)
                    {
                        ISample sample = _selectedClient?.SampleCache.LastOrDefault();
                        if (sample != null)
                        {
                            _valueDisplay.SetSample(sample);
                        }
                    }
                }
            }
        }
    }

    public SensorsDisplayView()
    {
        _updateTimer = new Timer(UpdateGui);
        _updateTimer.Change(0, 100);

        ColumnDefinition[] columms = {
            new(200),
            new(GridLength.Star)
        };

        RowDefinition[] rows = {
            new(40),
            new(GridLength.Star)
        };

        this.RowDefinitions(rows)
            .ColumnDefinitions(columms)
            .Margin(2);

        _selectedSensorLabel = new Label("Select a sensor")
            .Column(1)
            .FontSize(10);

        Add(_selectedSensorLabel);

        _usedSensorsCollectionView = new CollectionView()
            .SelectionMode(SelectionMode.Single)
            .BackgroundColor(API.Instance.Foreground.WithAlpha(0.7f))
            .Row(1)
            .Column(0)
            .Margin(2)
            .OnSelectionChanged(UsedSensor_Selected)
            .ItemTemplate(new SensorsDataTemplate());

        Add(_usedSensorsCollectionView);

        RowDefinition[] viewRows =
        {
            new (30),
            new (30),
            new (30),
            new (200),
            new (200),
            new (GridLength.Star),
        };

        ColumnDefinition[] viewColumns =
        {
            new(130),
            new(GridLength.Star)
        };
        Grid views = new Grid()
            .Column(1)
            .Row(1)
            .RowDefinitions(viewRows)
            .ColumnDefinitions(viewColumns);

        _valueDisplay = new SensorValueDisplay()
            .RowSpan(3)
            .Column(0)
            .WidthRequest(100)
            .HeightRequest(100);
        views.Add(_valueDisplay);

        Add(views);

        this.OnLoaded(OnLoaded);
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        ObservableCollection<SensorItem> itemsUsed = new(SensorsConfigurationView.GetSavedSensors());
        _usedSensorsCollectionView.ItemsSource = itemsUsed;
    }

    private async void UsedSensor_Selected(object? sender, SelectionChangedEventArgs e)
    {
        if (_usedSensorsCollectionView.SelectedItem is not SensorItem item) return;

        Type? resultType = item.SensorType switch
        {
            nameof(AccelerationSensor) => typeof(List<AccelerationSample>),
            nameof(BarometerSensor) => typeof(List<BarometerSample>),
            nameof(CompassSensor) => typeof(List<CompassSample>),
            nameof(GyroscopeSensor) => typeof(List<GyroscopeSample>),
            nameof(MagneticSensor) => typeof(List<MagneticSample>),
            nameof(OrientationSensor) => typeof(List<OrientationSample>),
            _ => null
        };

        _selectedSensorLabel.Text = $"{item.Name}@{item.Location}";
        if (_clients.ContainsKey(item))
        {
            _selectedClient?.Stop();
            _selectedClient = _clients[item];
#pragma warning disable CS4014
            _selectedClient.Start();
#pragma warning restore CS4014
            if (resultType != null)
            {
                _valueDisplay.SetSampleType(resultType.GenericTypeArguments.First());
            }

            return;
        }

        if (!Uri.TryCreate(item.Location, UriKind.Absolute, out Uri? url))
        {
            return;
        }

        if (resultType == null)
        {
            return;
        }

        DownloadClient client = new(url, "", resultType, 500);
        _clients.Add(item, client);
#pragma warning disable CS4014
        client.Start();
#pragma warning restore CS4014
        _valueDisplay.SetSampleType(resultType.GenericTypeArguments.First());
    }
}
