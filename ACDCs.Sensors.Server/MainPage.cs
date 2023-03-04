// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace ACDCs.Sensors.Server;

using Services;
using Sharp.UI;

public class MainPage : ContentPage
{
    // ReSharper disable once RedundantNameQualifier
    private ACDCs.Sensors.API.SensorServer? _server;

    // ReSharper disable once NotAccessedField.Local
    private readonly Timer _timer;

    private Grid? _mainGrid;

    // ReSharper disable once NotAccessedField.Local
    private Button? _startButton;

    // ReSharper disable once NotAccessedField.Local
    private Button? _stopButton;

    private CheckBox? AccelerometerAvailableCheckbox { get; set; }

    private Label? AccelerometerCacheLabel { get; set; } = new();

    private CheckBox? AccelerometerRunningCheckbox { get; set; }

    private Label? AccelerometerSpeedLabel { get; set; } = new();

    private CheckBox? AllAvailableCheckbox { get; set; }

    private CheckBox? BarometerAvailableCheckbox { get; set; }

    private CheckBox? BarometerRunningCheckbox { get; set; }

    private Label? BarometerSensorCacheLabel { get; set; } = new();

    private Label? BarometerSensorSpeedLabel { get; set; } = new();

    private CheckBox? CompassAvailableCheckbox { get; set; }

    private CheckBox? CompassRunningCheckbox { get; set; }

    private Label? CompassSensorCacheLabel { get; set; } = new();

    private Label? CompassSensorSpeedLabel { get; set; } = new();

    private CheckBox? GyroscopeAvailableCheckbox { get; set; }

    private CheckBox? GyroscopeRunningCheckbox { get; set; }

    private Label? GyroscopeSensorCacheLabel { get; set; } = new();

    private Label? GyroscopeSensorSpeedLabel { get; set; } = new();

    private Label IpLabel { get; set; } = new();

    private CheckBox? MagnetometerAvailableCheckbox { get; set; }

    private Label? MagnetometerCacheLabel { get; set; } = new();

    private CheckBox? MagnetometerRunningCheckbox { get; set; }

    private Label? MagnetometerSpeedLabel { get; set; } = new();

    private Label? NetworkConnectionsLabel { get; set; } = new();

    private Label? NetworkDeliveryLabel { get; set; } = new();

    private CheckBox? OrientationSensorAvailableCheckbox { get; set; }

    private Label? OrientationSensorCacheLabel { get; set; } = new();

    private CheckBox? OrientationSensorRunningCheckbox { get; set; }

    private Label? OrientationSensorSpeedLabel { get; set; } = new();

    private Entry? PortEntry { get; set; }

    public MainPage()
    {
        InitializeComponent();

        _timer = new Timer(UpdateGui, null, 0, 1000);
    }

    private static string GetLocalIpAddress()
    {
        NetService netService = new();
        string ipAddress = netService.ConvertHostIP();
        return $"http://{ipAddress}:";
    }

    private void AccelerometerAvailableCheckbox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (AccelerometerAvailableCheckbox == null || _server == null)
        {
            return;
        }

        _server.AccelerationSupported = AccelerometerAvailableCheckbox.IsChecked;
    }

    private void AllAvailableCheckbox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (sender is not CheckBox box) return;

        if (MagnetometerAvailableCheckbox != null)
        {
            MagnetometerAvailableCheckbox.IsChecked = MagnetometerAvailableCheckbox.IsEnabled && box.IsChecked;
        }

        if (OrientationSensorAvailableCheckbox != null)
        {
            OrientationSensorAvailableCheckbox.IsChecked =
                OrientationSensorAvailableCheckbox.IsEnabled && box.IsChecked;
        }

        if (AccelerometerAvailableCheckbox != null)
        {
            AccelerometerAvailableCheckbox.IsChecked = AccelerometerAvailableCheckbox.IsEnabled && box.IsChecked;
        }

        if (BarometerAvailableCheckbox != null)
        {
            BarometerAvailableCheckbox.IsChecked = BarometerAvailableCheckbox.IsEnabled && box.IsChecked;
        }

        if (CompassAvailableCheckbox != null)
        {
            CompassAvailableCheckbox.IsChecked = CompassAvailableCheckbox.IsEnabled && box.IsChecked;
        }

        if (GyroscopeAvailableCheckbox != null)
        {
            GyroscopeAvailableCheckbox.IsChecked = GyroscopeAvailableCheckbox.IsEnabled && box.IsChecked;
        }
    }

    private void BarometerAvailableCheckbox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (BarometerAvailableCheckbox == null)
        {
            return;
        }

        if (_server != null)
        {
            _server.BarometerSupported = BarometerAvailableCheckbox.IsChecked;
        }
    }

    private CheckBox Checkbox(int row, int column = 0)
    {
        CheckBox box = new CheckBox()
            .Row(row)
            .Column(column)
            .WidthRequest(32)
            .HeightRequest(32);

        _mainGrid?.Add(box);

        return box;
    }

    private ColumnDefinition Column(int width = 0)
    {
        return width > 0 ? new ColumnDefinition(width) : new ColumnDefinition(GridLength.Star);
    }

    private void CompassAvailableCheckbox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (CompassAvailableCheckbox == null)
        {
            return;
        }

        if (_server != null)
        {
            _server.CompassSupported = CompassAvailableCheckbox.IsChecked;
        }
    }

    private void GyroscopeAvailableCheckbox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (GyroscopeAvailableCheckbox == null)
        {
            return;
        }

        if (_server != null)
        {
            _server.GyroscopeSupported = GyroscopeAvailableCheckbox.IsChecked;
        }
    }

    private void InitializeComponent()
    {
        RowDefinition[] rows =
        {
            Row(40),
            Row(32),
            Row(40),
            Row(32),
            Row(32),
            Row(32),
            Row(32),
            Row(32),
            Row(32),
            Row(32),
            Row(32),
            Row(),
        };

        ColumnDefinition[] columns =
        {
            Column(32),
            Column(32),
            Column(),
            Column(120),
            Column(120),
        };

        _mainGrid = new Grid()
            .RowDefinitions(rows)
            .ColumnDefinitions(columns)
            .Padding(0)
            .ColumnSpacing(0)
            .RowSpacing(0)
            .Margin(2);

        _mainGrid.Add(new Label("ACDCs.Sensors.Server")
            .FontSize(25)
            .FontAttributes(FontAttributes.Bold)
            .ColumnSpan(4));

        AllAvailableCheckbox = Checkbox(1)
            .OnCheckedChanged(AllAvailableCheckbox_CheckedChanged);

        _mainGrid.Add(new Label("(de)select all")
            .Row(1)
            .VerticalTextAlignment(TextAlignment.Center)
            .ColumnSpan(3)
            .Column(1));

        MagnetometerAvailableCheckbox = Checkbox(3)
            .OnCheckedChanged(MagnetometerAvailableCheckbox_CheckedChanged);

        PutLabels(MagnetometerSpeedLabel, MagnetometerCacheLabel, 3, "Magnetometer");

        OrientationSensorAvailableCheckbox = Checkbox(4)
            .OnCheckedChanged(OrientationSensorAvailableCheckbox_CheckedChanged);

        PutLabels(OrientationSensorSpeedLabel, OrientationSensorCacheLabel, 4, "Orientation");

        AccelerometerAvailableCheckbox = Checkbox(5)
            .OnCheckedChanged(AccelerometerAvailableCheckbox_CheckedChanged);

        PutLabels(AccelerometerSpeedLabel, AccelerometerCacheLabel, 5, "Accelerometer");

        BarometerAvailableCheckbox = Checkbox(6)
            .OnCheckedChanged(BarometerAvailableCheckbox_CheckedChanged);

        PutLabels(BarometerSensorSpeedLabel, BarometerSensorCacheLabel, 6, "Barometer");

        CompassAvailableCheckbox = Checkbox(7)
            .OnCheckedChanged(CompassAvailableCheckbox_CheckedChanged);

        PutLabels(CompassSensorSpeedLabel, CompassSensorCacheLabel, 7, "Compass");

        GyroscopeAvailableCheckbox = Checkbox(8)
            .OnCheckedChanged(GyroscopeAvailableCheckbox_CheckedChanged);

        PutLabels(GyroscopeSensorSpeedLabel, GyroscopeSensorCacheLabel, 8, "Gyroscope");

        MagnetometerRunningCheckbox = Checkbox(3, 1);
        OrientationSensorRunningCheckbox = Checkbox(4, 1);
        AccelerometerRunningCheckbox = Checkbox(5, 1);
        BarometerRunningCheckbox = Checkbox(6, 1);
        CompassRunningCheckbox = Checkbox(7, 1);
        GyroscopeRunningCheckbox = Checkbox(8, 1);

        _startButton = new Button("Start server")
            .Row(1)
            .Column(3)
            .OnClicked(StartServer_Clicked);
        _mainGrid.Add(_startButton);

        _stopButton = new Button("Stop server")
            .Row(1)
            .Column(4)
            .OnClicked(StopServer_Clicked);
        _mainGrid.Add(_stopButton);

        _mainGrid.Add(
            IpLabel
                .Text(GetLocalIpAddress())
                .FontSize(14)
                .Row(10)
                .ColumnSpan(3));

        PortEntry = new Entry("port")
            .Text("5000")
            .Row(10)
            .Column(2)
            .WidthRequest(60)
            .HorizontalOptions(LayoutOptions.End);
        _mainGrid.Add(PortEntry);

        PutLabels(NetworkDeliveryLabel, NetworkConnectionsLabel, 9, "Network");

        Content = _mainGrid;
    }

    private void MagnetometerAvailableCheckbox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (MagnetometerAvailableCheckbox == null)
        {
            return;
        }

        if (_server != null)
        {
            _server.MagnetometerSupported = MagnetometerAvailableCheckbox.IsChecked;
        }
    }

    private void OrientationSensorAvailableCheckbox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (OrientationSensorAvailableCheckbox == null)
        {
            return;
        }

        if (_server != null)
        {
            _server.OrientationSupported = OrientationSensorAvailableCheckbox.IsChecked;
        }
    }

    private void PutLabels(Label? label1, Label? label2, int row, string label)
    {
        StackLayout labelLayout = new StackLayout { label1, label2 }
            .Row(row)
            .Column(3)
            .Orientation(StackOrientation.Vertical);

        _mainGrid?.Add(labelLayout);

        _mainGrid?.Add(new Label(label)
            .Row(row)
            .Column(2)
            .VerticalTextAlignment(TextAlignment.Center)
        );
    }

    private RowDefinition Row(int height = 0)
    {
        return height > 0 ? new RowDefinition(height) : new RowDefinition(GridLength.Star);
    }

    private void StartServer_Clicked(object? sender, EventArgs e)
    {
        int port = 5000;
        // ReSharper disable once InlineOutVariableDeclaration
        if (int.TryParse(PortEntry?.Text, out int editPort))
        {
            port = editPort;
        }

        IpLabel.Text = GetLocalIpAddress();

        _server = new API.SensorServer(port);
        _server.Start();

        if (MagnetometerAvailableCheckbox != null)
        {
            MagnetometerAvailableCheckbox.IsEnabled =
                MagnetometerAvailableCheckbox.IsChecked = _server.MagnetometerSupported;
        }

        if (OrientationSensorAvailableCheckbox != null)
        {
            OrientationSensorAvailableCheckbox.IsEnabled =
                OrientationSensorAvailableCheckbox.IsChecked = _server.OrientationSupported;
        }

        if (AccelerometerAvailableCheckbox != null)
        {
            AccelerometerAvailableCheckbox.IsEnabled =
                AccelerometerAvailableCheckbox.IsChecked = _server.AccelerationSupported;
        }

        if (BarometerAvailableCheckbox != null)
        {
            BarometerAvailableCheckbox.IsEnabled = BarometerAvailableCheckbox.IsChecked = _server.BarometerSupported;
        }

        if (CompassAvailableCheckbox != null)
        {
            CompassAvailableCheckbox.IsEnabled = CompassAvailableCheckbox.IsChecked = _server.CompassSupported;
        }

        if (GyroscopeAvailableCheckbox != null)
        {
            GyroscopeAvailableCheckbox.IsEnabled = GyroscopeAvailableCheckbox.IsChecked = _server.GyroscopeSupported;
        }

        if (MagnetometerRunningCheckbox != null)
        {
            MagnetometerRunningCheckbox.IsChecked = _server.MagneticWorkerStarted;
        }

        if (OrientationSensorRunningCheckbox != null)
        {
            OrientationSensorRunningCheckbox.IsChecked = _server.OrientationWorkerStarted;
        }

        if (AccelerometerRunningCheckbox != null)
        {
            AccelerometerRunningCheckbox.IsChecked = _server.AccelerationWorkerStarted;
        }

        if (BarometerRunningCheckbox != null)
        {
            BarometerRunningCheckbox.IsChecked = _server.BarometerWorkerStarted;
        }

        if (CompassRunningCheckbox != null)
        {
            CompassRunningCheckbox.IsChecked = _server.CompassWorkerStarted;
        }

        if (GyroscopeRunningCheckbox != null)
        {
            GyroscopeRunningCheckbox.IsChecked = _server.GyroscopeWorkerStarted;
        }
    }

    private void StopServer_Clicked(object? sender, EventArgs e)
    {
        _server?.Stop();
    }

    private void Update()
    {
        if (_server == null) return;
        if (NetworkConnectionsLabel != null)
        {
            NetworkConnectionsLabel.Text = _server?.NetworkConnectionLabel;
        }

        if (NetworkDeliveryLabel != null)
        {
            NetworkDeliveryLabel.Text = _server?.NetworkDeliveryLabel;
        }

        if (MagnetometerSpeedLabel != null)
        {
            MagnetometerSpeedLabel.Text = _server?.MagnetometerSpeedLabelText;
        }

        if (OrientationSensorSpeedLabel != null)
        {
            OrientationSensorSpeedLabel.Text = _server?.OrientationSensorSpeedLabelText;
        }

        if (AccelerometerSpeedLabel != null)
        {
            AccelerometerSpeedLabel.Text = _server?.AccelerometerSpeedLabelText;
        }

        if (BarometerSensorSpeedLabel != null)
        {
            BarometerSensorSpeedLabel.Text = _server?.BarometerSensorSpeedLabelText;
        }

        if (CompassSensorSpeedLabel != null)
        {
            CompassSensorSpeedLabel.Text = _server?.CompassSensorSpeedLabelText;
        }

        if (GyroscopeSensorSpeedLabel != null)
        {
            GyroscopeSensorSpeedLabel.Text = _server?.GyroscopeSensorSpeedLabelText;
        }

        if (MagnetometerCacheLabel != null)
        {
            MagnetometerCacheLabel.Text = _server?.MagnetometerCacheLabelText;
        }

        if (OrientationSensorCacheLabel != null)
        {
            OrientationSensorCacheLabel.Text = _server?.OrientationSensorCacheLabelText;
        }

        if (AccelerometerCacheLabel != null)
        {
            AccelerometerCacheLabel.Text = _server?.AccelerometerCacheLabelText;
        }

        if (BarometerSensorCacheLabel != null)
        {
            BarometerSensorCacheLabel.Text = _server?.BarometerSensorCacheLabelText;
        }

        if (CompassSensorCacheLabel != null)
        {
            CompassSensorCacheLabel.Text = _server?.CompassSensorCacheLabelText;
        }

        if (GyroscopeSensorCacheLabel != null)
        {
            GyroscopeSensorCacheLabel.Text = _server?.GyroscopeSensorCacheLabelText;
        }
    }

    private void UpdateGui(object? state)
    {
#if ANDROID
        if (MainThread.IsMainThread)
        {
            return;
        }

        MainThread.InvokeOnMainThreadAsync(Update);
#endif
#if WINDOWS
        Update();
#endif
    }
}
