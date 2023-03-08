namespace ACDCs.API.Core.Components.Sensors;

using System.Collections.ObjectModel;
using System.Linq;
using ACDCs.Sensors.API;
using ACDCs.Sensors.API.Client;
using ACDCs.Sensors.API.Interfaces;
using ACDCs.Sensors.API.Samples;
using ACDCs.Sensors.API.Sensors;
using Instance;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using Sharp.UI;
using SkiaSharp;

public class SensorsDisplayView : Grid
{
    private readonly Label _selectedSensorLabel;
    private readonly Timer _updateTimer;
    private readonly CollectionView _usedSensorsCollectionView;
    private readonly SensorValueDisplay _valueDisplay;
    private Axis _axisY;
    private DownloadClient? _selectedClient;
    private LineSeries<SeriesSample> _seriesH;
    private LineSeries<SeriesSample> _seriesX;
    private LineSeries<SeriesSample> _seriesY;
    private LineSeries<SeriesSample> _seriesZ;
    private CartesianChart? _valueChart;

    public SensorsDisplayView()
    {
        _updateTimer = new Timer(UpdateGui);
        _updateTimer.Change(0, 100);

        ColumnDefinition[] columms = {
            new(130),
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

        AddValueChart(views);

        Add(views);

        this.OnLoaded(OnLoaded);
    }

    private static LineSeries<SeriesSample> GetSeries(SKColor color)
    {
        return new LineSeries<SeriesSample>
        {
            Values = new ObservableCollection<SeriesSample>(),
            DataLabelsPaint = null,
            DataLabelsPadding = new LiveChartsCore.Drawing.Padding(2),
            DataLabelsPosition = DataLabelsPosition.End,
            Stroke = new SolidColorPaint(color)
            {
                StrokeThickness = 3
            },
            GeometryStroke = null,
            GeometrySize = 0,
            EnableNullSplitting = false,
            GeometryFill = null,
            Mapping = Mapping
        };
    }

    private static void Mapping(SeriesSample sample, ChartPoint chartPoint)
    {
        chartPoint.PrimaryValue = sample.Value;
        chartPoint.SecondaryValue = sample.Time.Ticks;
    }

    private static double SampleValues(ISample? update, double sampleValueX, ref double sampleValueY,
        ref double sampleValueZ, ref double sampleValueH)
    {
        switch (update)
        {
            case AccelerationSample msample:
                sampleValueX = msample.Sample.X;
                sampleValueY = msample.Sample.Y;
                sampleValueZ = msample.Sample.Z;
                break;

            case BarometerSample msample:
                sampleValueX = msample.Sample;
                break;

            case CompassSample msample:
                sampleValueX = msample.Sample;
                break;

            case GyroscopeSample msample:
                sampleValueX = msample.Sample.X;
                sampleValueY = msample.Sample.Y;
                sampleValueZ = msample.Sample.Z;
                break;

            case MagneticSample msample:
                sampleValueX = msample.Sample.X;
                sampleValueY = msample.Sample.Y;
                sampleValueZ = msample.Sample.Z;
                break;

            case OrientationSample msample:
                sampleValueX = msample.Sample.X;
                sampleValueY = msample.Sample.Y;
                sampleValueZ = msample.Sample.Z;
                sampleValueH = msample.Sample.W;
                break;
        }

        return sampleValueX;
    }

    private void AddValueChart(Grid targetGrid)
    {
        _valueChart = new CartesianChart()
            .Margin(0)
            .Padding(0);

        _seriesX = GetSeries(SKColors.Red);
        _seriesY = GetSeries(SKColors.Green);
        _seriesZ = GetSeries(SKColors.Blue);
        _seriesH = GetSeries(SKColors.Yellow);

        _valueChart.Series = new ObservableCollection<ISeries> { _seriesX, _seriesY, _seriesZ };
        _valueChart.XAxes = new List<Axis>
        {
            new()
            {
                Name = "Time",
                NamePadding = new LiveChartsCore.Drawing.Padding(2),
                Padding = new LiveChartsCore.Drawing.Padding(2),
                NameTextSize= 12,
                NamePaint = new SolidColorPaint(SKColors.White),
                LabelsPaint = new SolidColorPaint(SKColors.White),
                TextSize = 12,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                {
                    StrokeThickness = 2
                }
            }
        };

        _axisY = new Axis
        {
            NameTextSize = 12,
            Name = "Power (μT)",
            NamePadding = new LiveChartsCore.Drawing.Padding(2),
            Padding = new LiveChartsCore.Drawing.Padding(2),
            NamePaint = new SolidColorPaint(SKColors.Yellow),
            LabelsPaint = new SolidColorPaint(SKColors.Yellow),
            TextSize = 12,
            SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
            {
                StrokeThickness = 2,
                PathEffect = new DashEffect(new float[] { 3, 3 })
            },
        };

        _valueChart.YAxes = new List<Axis>
        {
            _axisY
        };

        targetGrid.Add(
            _valueChart
            .Row(3)
            .Column(0)
            .ColumnSpan(2));
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        ObservableCollection<SensorItem> itemsUsed = new(SensorsConfigurationView.GetSavedSensors());
        _usedSensorsCollectionView.ItemsSource = itemsUsed;
    }

    private void UpdateGui(object? state)
    {
        try
        {
            if (_selectedClient?.SampleCache == null || _selectedClient == null || _selectedClient?.SampleCache == null)
            {
                return;
            }

            ISample? sample = _selectedClient?.SampleCache.LastOrDefault();

            if (sample != null)
            {
                _valueDisplay.SetSample(sample);
            }

            if (_seriesX.Values is not ObservableCollection<SeriesSample> valuesX) return;
            if (_seriesY.Values is not ObservableCollection<SeriesSample> valuesY) return;
            if (_seriesZ.Values is not ObservableCollection<SeriesSample> valuesZ) return;
            if (_seriesH.Values is not ObservableCollection<SeriesSample> valuesH) return;
            var lastSample = valuesX.LastOrDefault();

            List<ISample?> updates = null;
            if (lastSample == null)
            {
                updates = _selectedClient?.SampleCache
                    .OrderByDescending(x => x.Time)
                    .Take(1000)
                    .OrderBy(x => x.Time)
                    .ToList();
            }
            else
            {
                updates = _selectedClient?.SampleCache
                    .Where(s => s.Time > lastSample.Time)
                    .OrderBy(x => x.Time)
                    .ToList();
            }

            if (updates == null)
            {
                return;
            }

            foreach (ISample? update in updates)
            {
                double sampleValueX = 0;
                double sampleValueY = 0;
                double sampleValueZ = 0;
                double sampleValueH = 0;

                sampleValueX = SampleValues(update, sampleValueX, ref sampleValueY, ref sampleValueZ, ref sampleValueH);

                valuesX.Add(new SeriesSample(sampleValueX, update.Time));
                valuesY.Add(new SeriesSample(sampleValueY, update.Time));
                valuesZ.Add(new SeriesSample(sampleValueZ, update.Time));
                valuesH.Add(new SeriesSample(sampleValueH, update.Time));

                if (valuesX.Count > 1000)
                    valuesX.RemoveAt(0);
                if (valuesY.Count > 1000)
                    valuesY.RemoveAt(0);
                if (valuesZ.Count > 1000)
                    valuesZ.RemoveAt(0);
                if (valuesH.Count > 1000)
                    valuesH.RemoveAt(0);
            }
        }
        catch (Exception ex)
        {
            // ignored
        }
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

        (_seriesX.Values as ObservableCollection<SeriesSample>).Clear();
        (_seriesY.Values as ObservableCollection<SeriesSample>).Clear();
        (_seriesZ.Values as ObservableCollection<SeriesSample>).Clear();
        (_seriesH.Values as ObservableCollection<SeriesSample>).Clear();

        switch (item.SensorType)
        {
            case nameof(AccelerationSensor):
                _seriesX.IsVisible = true;
                _seriesY.IsVisible = true;
                _seriesZ.IsVisible = true;
                _seriesH.IsVisible = false;
                _axisY.Name = "G";
                break;

            case nameof(BarometerSensor):
                _seriesX.IsVisible = true;
                _seriesY.IsVisible = false;
                _seriesZ.IsVisible = false;
                _seriesH.IsVisible = false;
                _axisY.Name = "uBar";
                break;

            case nameof(CompassSensor):
                _seriesX.IsVisible = true;
                _seriesY.IsVisible = false;
                _seriesZ.IsVisible = false;
                _seriesH.IsVisible = false;
                _axisY.Name = "Degree";
                break;

            case nameof(GyroscopeSensor):
                _seriesX.IsVisible = true;
                _seriesY.IsVisible = true;
                _seriesZ.IsVisible = true;
                _seriesH.IsVisible = false;
                _axisY.Name = "Dir";
                break;

            case nameof(MagneticSensor):
                _seriesX.IsVisible = true;
                _seriesY.IsVisible = true;
                _seriesZ.IsVisible = true;
                _seriesH.IsVisible = false;
                _axisY.Name = "uT";
                break;

            case nameof(OrientationSensor):
                _seriesX.IsVisible = true;
                _seriesY.IsVisible = true;
                _seriesZ.IsVisible = true;
                _seriesH.IsVisible = true;
                _axisY.Name = "Dir";
                break;
        }

        _selectedSensorLabel.Text = $"{item.Name}@{item.Location}";

        if (!Uri.TryCreate(item.Location, UriKind.Absolute, out Uri? url))
        {
            return;
        }

        if (resultType == null)
        {
            return;
        }

        _selectedClient?.Stop();

        DownloadClient client = new(url, "", resultType, 100);
#pragma warning disable CS4014
        client.Start();
        _selectedClient = client;
#pragma warning restore CS4014
        _valueDisplay.SetSampleType(resultType.GenericTypeArguments.First());
    }
}
