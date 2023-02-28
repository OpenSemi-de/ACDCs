namespace ACDCs.Sensors.Server;

using System.Net;
using System.Net.Sockets;
using Microsoft.Maui.Controls;

public partial class MainPage : ContentPage
{
#if ANDROID
    private IBackgroundService _service = new BackgroundService();
#endif

    // ReSharper disable once RedundantNameQualifier
    private readonly ACDCs.Sensors.API.SensorServer _server = new();

    // ReSharper disable once NotAccessedField.Local
    private readonly Timer _timer;

    public MainPage()
    {
        InitializeComponent();

        _timer = new Timer(UpdateGui, null, 0, 1000);

        MagnetometerAvailableCheckbox.IsChecked = _server.MagnetometerSupported;
        OrientationSensorAvailableCheckbox.IsChecked = _server.OrientationSupported;
        AccelerometerAvailableCheckbox.IsChecked = _server.AccelerationSupported;
        BarometerAvailableCheckbox.IsChecked = _server.BarometerSupported;
        CompassAvailableCheckbox.IsChecked = _server.CompassSupported;
        GyroscopeAvailableCheckbox.IsChecked = _server.GyroscopeSupported;
    }

    private static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork && !ip.ToString().StartsWith("127."))
            {
                return ip.ToString();
            }
        }

        return "No IP found.";
    }

    private void AccelerometerAvailableCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _server.AccelerationSupported = AccelerometerAvailableCheckbox.IsChecked;
    }

    private void AllAvailableCheckbox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
    }

    private void BarometerAvailableCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _server.BarometerSupported = BarometerAvailableCheckbox.IsChecked;
    }

    private void CompassAvailableCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _server.CompassSupported = CompassAvailableCheckbox.IsChecked;
    }

    private void GyroscopeAvailableCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _server.GyroscopeSupported = GyroscopeAvailableCheckbox.IsChecked;
    }

    private void MagnetometerAvailableCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _server.MagnetometerSupported = MagnetometerAvailableCheckbox.IsChecked;
    }

    private void OrientationSensorAvailableCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _server.OrientationSupported = OrientationSensorAvailableCheckbox.IsChecked;
    }

    private void StartServer_Clicked(object sender, EventArgs e)
    {
        // ReSharper disable once InlineOutVariableDeclaration
        if (!int.TryParse(PortEntry.Text, out int _))
        {
        }
        IpLabel.Text = GetLocalIPAddress();
        _server.Start();

        MagnetometerRunningCheckbox.IsChecked = _server.MagneticWorkerStarted;
        OrientationSensorRunningCheckbox.IsChecked = _server.OrientationWorkerStarted;
        AccelerometerRunningCheckbox.IsChecked = _server.AccelerationWorkerStarted;
        BarometerRunningCheckbox.IsChecked = _server.BarometerWorkerStarted;
        CompassRunningCheckbox.IsChecked = _server.CompassWorkerStarted;
        GyroscopeRunningCheckbox.IsChecked = _server.GyroscopeWorkerStarted;

#if ANDROID
        _service.Start();
#endif
    }

    private void StopServer_Clicked(object sender, EventArgs e)
    {
        _server.Stop();
    }

    private void UpdateGui(object? state)
    {
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            MagnetometerSpeedLabel.Text = _server.MagnetometerSpeedLabelText;
            OrientationSensorSpeedLabel.Text = _server.OrientationSensorSpeedLabelText;
            AccelerometerSpeedLabel.Text = _server.AccelerometerSpeedLabelText;
            BarometerSensorSpeedLabel.Text = _server.BarometerSensorSpeedLabelText;
            CompassSensorSpeedLabel.Text = _server.CompassSensorSpeedLabelText;
            GyroscopeSensorSpeedLabel.Text = _server.GyroscopeSensorSpeedLabelText;
            MagnetometerCacheLabel.Text = _server.MagnetometerCacheLabelText;
            OrientationSensorCacheLabel.Text = _server.OrientationSensorCacheLabelText;
            AccelerometerCacheLabel.Text = _server.AccelerometerCacheLabelText;
            BarometerSensorCacheLabel.Text = _server.BarometerSensorCacheLabelText;
            CompassSensorCacheLabel.Text = _server.CompassSensorCacheLabelText;
            GyroscopeSensorCacheLabel.Text = _server.GyroscopeSensorCacheLabelText;
        });
    }
}
