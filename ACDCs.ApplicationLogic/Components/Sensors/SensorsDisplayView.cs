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
using Image = Sharp.UI.Image;

public class SensorsDisplayView : Grid
{
    private readonly FftWorker _fftWorkerH;
    private readonly FftWorker _fftWorkerX;
    private readonly FftWorker _fftWorkerY;
    private readonly FftWorker _fftWorkerZ;
    private readonly Label _selectedSensorLabel;
    private readonly Timer _updateFftTimer;
    private readonly Timer _updateTimer;
    private readonly CollectionView _usedSensorsCollectionView;
    private readonly SensorValueDisplay _valueDisplay;
    private Axis? _axisFftY;
    private Axis? _axisY;
    private CartesianChart? _fftChart;
    private DownloadClient? _selectedClient;
    private LineSeries<FftInfo>? _seriesFftH;
    private LineSeries<FftInfo>? _seriesFftX;
    private LineSeries<FftInfo>? _seriesFftY;
    private LineSeries<FftInfo>? _seriesFftZ;
    private LineSeries<SeriesSample>? _seriesH;
    private LineSeries<SeriesSample>? _seriesX;
    private LineSeries<SeriesSample>? _seriesY;
    private LineSeries<SeriesSample>? _seriesZ;
    private Image _spectogramImage;
    private CartesianChart? _valueChart;

    public SensorsDisplayView()
    {
        _updateTimer = new Timer(UpdateGui);
        _updateFftTimer = new Timer(UpdateFft);
        _updateTimer.Change(0, 200);
        _updateFftTimer.Change(0, 200);

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
            new (GridLength.Star),
            new (260),
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
        AddFftChart(views);

        Add(views);

        _fftWorkerX = new FftWorker();
        _fftWorkerY = new FftWorker();
        _fftWorkerZ = new FftWorker();
        _fftWorkerH = new FftWorker();

        this.OnLoaded(OnLoaded);
        this.OnUnloaded(OnUnloaded);
    }

    private static void GetDataForFft(LineSeries<FftInfo>? series, FftWorker? worker)
    {
        if (worker == null) return;
        if (series?.Values is not ObservableCollection<FftInfo> values) return;
        // values.Clear();
        if (!worker.OutputQueue.TryDequeue(out FftInfoPacket? pack))
        {
            return;
        }

        if (pack.Count != values.Count)
        {
            values.Clear();

            foreach (var info in pack)
            {
                values.Add(info);
            }
        }
        else
        {
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = pack[i];
            }
        }
    }

    private static LineSeries<SeriesSample> GetSeries(SKColor color)
    {
        return new LineSeries<SeriesSample>
        {
            AnimationsSpeed = TimeSpan.FromMilliseconds(250),
            Values = new ObservableCollection<SeriesSample>(),
            DataLabelsPaint = null,
            DataLabelsPadding = new LiveChartsCore.Drawing.Padding(2),
            DataLabelsPosition = DataLabelsPosition.End,
            Stroke = new SolidColorPaint(color)
            {
                StrokeThickness = 2,
                IsAntialias = false
            },
            GeometryStroke = null,
            GeometrySize = 0,
            EnableNullSplitting = false,
            GeometryFill = null,
            Mapping = Mapping
        };
    }

    private static LineSeries<FftInfo> GetSeriesFft(SKColor color)
    {
        return new LineSeries<FftInfo>
        {
            AnimationsSpeed = TimeSpan.FromMilliseconds(250),
            Values = new ObservableCollection<FftInfo>(),
            DataLabelsPaint = null,
            DataLabelsPadding = new LiveChartsCore.Drawing.Padding(2),
            DataLabelsPosition = DataLabelsPosition.End,
            Stroke = new SolidColorPaint(color)
            {
                StrokeThickness = 2,
                IsAntialias = false
            },
            GeometryStroke = null,
            GeometrySize = 0,
            EnableNullSplitting = false,
            GeometryFill = null,
            Mapping = MappingFft
        };
    }

    private static void Mapping(SeriesSample sample, ChartPoint chartPoint)
    {
        chartPoint.PrimaryValue = sample.Value;
        chartPoint.SecondaryValue = sample.Time.Ticks;
    }

    private static void MappingFft(FftInfo fft, ChartPoint chartPoint)
    {
        chartPoint.PrimaryValue = fft.Value;
        chartPoint.SecondaryValue = fft.Freq;
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

    private void AddFftChart(Grid targetGrid)
    {
        _fftChart = new CartesianChart()
            .Margin(0)
            .Padding(0);

        _seriesFftX = GetSeriesFft(SKColors.Red);
        _seriesFftY = GetSeriesFft(SKColors.Green);
        _seriesFftZ = GetSeriesFft(SKColors.Blue);
        _seriesFftH = GetSeriesFft(SKColors.Yellow);

        _fftChart.Series = new ObservableCollection<ISeries> { _seriesFftX, _seriesFftY, _seriesFftZ, _seriesFftH };
        _fftChart.XAxes = new List<Axis>
        {
            new()
            {
                Name = "Frequency",
                NamePadding = new LiveChartsCore.Drawing.Padding(2),
                Padding = new LiveChartsCore.Drawing.Padding(2),
                NameTextSize= 12,
                NamePaint = new SolidColorPaint(SKColors.White){IsAntialias = false},
                LabelsPaint = new SolidColorPaint(SKColors.White){IsAntialias = false},
                TextSize = 12,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                {
                    IsAntialias = false,
                    StrokeThickness = 2
                }
            }
        };

        _axisFftY = new Axis
        {
            Name = "dBi",
            NameTextSize = 12,
            NamePadding = new LiveChartsCore.Drawing.Padding(2),
            Padding = new LiveChartsCore.Drawing.Padding(2),
            NamePaint = new SolidColorPaint(SKColors.Yellow) { IsAntialias = false },
            LabelsPaint = new SolidColorPaint(SKColors.Yellow) { IsAntialias = false },
            TextSize = 12,
            SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
            {
                IsAntialias = false,
                StrokeThickness = 2,
                PathEffect = new DashEffect(new float[] { 3, 3 })
            },
        };

        _fftChart.YAxes = new List<Axis>
        {
            _axisFftY
        };

        targetGrid.Add(
            _fftChart
                .Row(4)
                .Column(0)
                .ColumnSpan(2));
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

        _valueChart.Series = new ObservableCollection<ISeries> { _seriesX!, _seriesY!, _seriesZ!, _seriesH! };
        _valueChart.XAxes = new List<Axis>
        {
            new()
            {
                Name = "Time",
                NamePadding = new LiveChartsCore.Drawing.Padding(2),
                Padding = new LiveChartsCore.Drawing.Padding(2),
                NameTextSize= 12,
                NamePaint = new SolidColorPaint(SKColors.White){IsAntialias = false},
                LabelsPaint = new SolidColorPaint(SKColors.White){IsAntialias = false},
                TextSize = 12,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                {
                    StrokeThickness = 2,
                    IsAntialias = false
                }
            }
        };

        _axisY = new Axis
        {
            NameTextSize = 12,
            NamePadding = new LiveChartsCore.Drawing.Padding(2),
            Padding = new LiveChartsCore.Drawing.Padding(2),
            NamePaint = new SolidColorPaint(SKColors.Yellow) { IsAntialias = false },
            LabelsPaint = new SolidColorPaint(SKColors.Yellow) { IsAntialias = false },
            TextSize = 12,
            SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
            {
                IsAntialias = false,
                StrokeThickness = 2,
                PathEffect = new DashEffect(new float[] { 3, 3 }),
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

    private void OnUnloaded(object? sender, EventArgs e)
    {
        _selectedClient?.Stop();
    }

    private void UpdateFft(object? state)
    {
        if (_seriesFftX != null && _seriesFftX.IsVisible) GetDataForFft(_seriesFftX, _fftWorkerX);
        if (_seriesFftY != null && _seriesFftY.IsVisible) GetDataForFft(_seriesFftY, _fftWorkerY);
        if (_seriesFftZ != null && _seriesFftZ.IsVisible) GetDataForFft(_seriesFftZ, _fftWorkerZ);
        if (_seriesFftH != null && _seriesFftH.IsVisible) GetDataForFft(_seriesFftH, _fftWorkerH);
    }

    private async void UpdateGui(object? state)
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

            if (_seriesX?.Values is not ObservableCollection<SeriesSample> valuesX) return;
            if (_seriesY?.Values is not ObservableCollection<SeriesSample> valuesY) return;
            if (_seriesZ?.Values is not ObservableCollection<SeriesSample> valuesZ) return;
            if (_seriesH?.Values is not ObservableCollection<SeriesSample> valuesH) return;
            var lastSample = valuesX.LastOrDefault();

            List<ISample?>? updates;
            if (lastSample == null)
            {
                updates = _selectedClient?.SampleCache
                    .Where(x => x != null)
                    .OrderByDescending(x => x!.Time)
                    .Take(1000)
                    .OrderBy(x => x!.Time)
                    .ToList();
            }
            else
            {
                updates = _selectedClient?.SampleCache
                    .Where(x => x != null)
                    .Where(s => s!.Time > lastSample.Time)
                    .OrderBy(x => x!.Time)
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

                if (update == null)
                {
                    continue;
                }

                valuesX.Add(new SeriesSample(sampleValueX, update.Time));
                valuesY.Add(new SeriesSample(sampleValueY, update.Time));
                valuesZ.Add(new SeriesSample(sampleValueZ, update.Time));
                valuesH.Add(new SeriesSample(sampleValueH, update.Time));

                _fftWorkerX.EnqueueSample(new FftSample(sampleValueX, update.Time));
                _fftWorkerY.EnqueueSample(new FftSample(sampleValueY, update.Time));
                _fftWorkerZ.EnqueueSample(new FftSample(sampleValueZ, update.Time));
                _fftWorkerH.EnqueueSample(new FftSample(sampleValueH, update.Time));
            }

            while (valuesX.Count > 1000)
                valuesX.RemoveAt(0);
            while (valuesY.Count > 1000)
                valuesY.RemoveAt(0);
            while (valuesZ.Count > 1000)
                valuesZ.RemoveAt(0);
            while (valuesH.Count > 1000)
                valuesH.RemoveAt(0);
        }
        catch (Exception ex)
        {
            // ignored
        }
    }

    private async void UsedSensor_Selected(object? sender, SelectionChangedEventArgs e)
    {
        await Task.CompletedTask;
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

        (_seriesX?.Values as ObservableCollection<SeriesSample>)?.Clear();
        (_seriesY?.Values as ObservableCollection<SeriesSample>)?.Clear();
        (_seriesZ?.Values as ObservableCollection<SeriesSample>)?.Clear();
        (_seriesH?.Values as ObservableCollection<SeriesSample>)?.Clear();

        switch (item.SensorType)
        {
            case nameof(AccelerationSensor):
                _seriesFftX.IsVisible = _seriesX.IsVisible = true;
                _seriesFftY.IsVisible = _seriesY.IsVisible = true;
                _seriesFftZ.IsVisible = _seriesZ.IsVisible = true;
                _seriesFftH.IsVisible = _seriesH.IsVisible = false;
                _axisY.Name = "G";
                break;

            case nameof(BarometerSensor):
                _seriesFftX.IsVisible = _seriesX.IsVisible = true;
                _seriesFftY.IsVisible = _seriesY.IsVisible = false;
                _seriesFftZ.IsVisible = _seriesZ.IsVisible = false;
                _seriesFftH.IsVisible = _seriesH.IsVisible = false;
                _axisY.Name = "μBar";
                break;

            case nameof(CompassSensor):
                _seriesFftX.IsVisible = _seriesX.IsVisible = true;
                _seriesFftY.IsVisible = _seriesY.IsVisible = false;
                _seriesFftZ.IsVisible = _seriesZ.IsVisible = false;
                _seriesFftH.IsVisible = _seriesH.IsVisible = false;
                _axisY.Name = "Degree";
                break;

            case nameof(GyroscopeSensor):
                _seriesFftX.IsVisible = _seriesX.IsVisible = true;
                _seriesFftY.IsVisible = _seriesY.IsVisible = true;
                _seriesFftZ.IsVisible = _seriesZ.IsVisible = true;
                _seriesFftH.IsVisible = _seriesH.IsVisible = false;
                _axisY.Name = "Degree";
                break;

            case nameof(MagneticSensor):
                _seriesFftX.IsVisible = _seriesX.IsVisible = true;
                _seriesFftY.IsVisible = _seriesY.IsVisible = true;
                _seriesFftZ.IsVisible = _seriesZ.IsVisible = true;
                _seriesFftH.IsVisible = _seriesH.IsVisible = false;
                _axisY.Name = "μT";
                break;

            case nameof(OrientationSensor):
                _seriesFftX.IsVisible = _seriesX.IsVisible = true;
                _seriesFftY.IsVisible = _seriesY.IsVisible = true;
                _seriesFftZ.IsVisible = _seriesZ.IsVisible = true;
                _seriesFftH.IsVisible = _seriesH.IsVisible = true;
                _axisY.Name = "Degree";
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
