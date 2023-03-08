namespace ACDCs.API.Core.Components.Sensors;

using ACDCs.Sensors.API.Interfaces;
using ACDCs.Sensors.API.Samples;

public class SensorValueDisplay : Grid
{
    private readonly Label _label1 = new(".");
    private readonly Label _label2 = new(".");
    private readonly Label _label3 = new(".");
    private readonly Label _label4 = new(".");
    private readonly Label _tlabel1 = new(".");
    private readonly Label _tlabel2 = new(".");
    private readonly Label _tlabel3 = new(".");
    private readonly Label _tlabel4 = new(".");

    public SensorValueDisplay()
    {
        ColumnDefinition[] columns =
        {
            new(30),
            new(100)
        };

        RowDefinition[] rows =
        {
            new(20),
            new(20),
            new(20),
            new(20)
        };

        this.RowDefinitions(rows)
            .ColumnDefinitions(columns);
        _tlabel1.TextColor(Colors.Red);
        _tlabel2.Row(1).TextColor(Colors.Green);
        _tlabel3.Row(2).TextColor(Colors.Blue);
        _tlabel4.Row(3).TextColor(Colors.Orange);

        _label1.Row(0).Column(1).FontSize(10);
        _label2.Row(1).Column(1).FontSize(10);
        _label3.Row(2).Column(1).FontSize(10);
        _label4.Row(3).Column(1).FontSize(10);

        Add(_tlabel1);
        Add(_tlabel2);
        Add(_tlabel3);
        Add(_tlabel4);
        Add(_label1);
        Add(_label2);
        Add(_label3);
        Add(_label4);
    }

    public void SetSample(ISample sample)
    {
        if (!MainThread.IsMainThread)
            MainThread.BeginInvokeOnMainThread(() => SetSampleRaw(sample));
        else SetSampleRaw(sample);
    }

    public void SetSampleRaw(ISample sample)
    {
        switch (sample)
        {
            case AccelerationSample accelerationSample:
                _label1.Text($"{accelerationSample.Sample.X}");
                _label2.Text($"{accelerationSample.Sample.Y}");
                _label3.Text($"{accelerationSample.Sample.Z}");
                break;

            case BarometerSample barometerSample:
                _label1.Text($"{barometerSample.Sample}");
                break;

            case CompassSample compassSample:
                _label1.Text($"{compassSample.Sample}");
                break;

            case GyroscopeSample gyroscopeSample:
                _label1.Text($"{gyroscopeSample.Sample.X}");
                _label2.Text($"{gyroscopeSample.Sample.Y}");
                _label3.Text($"{gyroscopeSample.Sample.Z}");
                break;

            case MagneticSample magneticSample:
                _label1.Text($"{magneticSample.Sample.X}");
                _label2.Text($"{magneticSample.Sample.Y}");
                _label3.Text($"{magneticSample.Sample.Z}");
                break;

            case OrientationSample orientationSample:
                _label1.Text($"{orientationSample.Sample.X}");
                _label2.Text($"{orientationSample.Sample.Y}");
                _label3.Text($"{orientationSample.Sample.Z}");
                _label4.Text($"{orientationSample.Sample.W}");
                break;
        }
    }

    public void SetSampleType(Type type)
    {
        if (type == typeof(AccelerationSample) || type == typeof(GyroscopeSample) || type == typeof(MagneticSample))
        {
            _tlabel1.Text("X");
            _tlabel2.Text("Y");
            _tlabel3.Text("Z");
            _label1.IsVisible = true;
            _label2.IsVisible = true;
            _label3.IsVisible = true;
            _label4.IsVisible = false;
            _tlabel1.IsVisible = true;
            _tlabel2.IsVisible = true;
            _tlabel3.IsVisible = true;
            _tlabel4.IsVisible = false;
        }

        if (type == typeof(BarometerSample) || type == typeof(CompassSample))
        {
            _tlabel1.Text("V");
            _label1.IsVisible = true;
            _label2.IsVisible = false;
            _label3.IsVisible = false;
            _label4.IsVisible = false;
            _tlabel1.IsVisible = true;
            _tlabel2.IsVisible = false;
            _tlabel3.IsVisible = false;
            _tlabel4.IsVisible = false;
        }

        if (type == typeof(OrientationSample))
        {
            _tlabel1.Text("X");
            _tlabel2.Text("Y");
            _tlabel3.Text("Z");
            _tlabel4.Text("W");
            _label1.IsVisible = true;
            _label2.IsVisible = true;
            _label3.IsVisible = true;
            _label4.IsVisible = true;
            _tlabel1.IsVisible = true;
            _tlabel2.IsVisible = true;
            _tlabel3.IsVisible = true;
            _tlabel4.IsVisible = true;
        }
    }
}
