namespace ACDCs.API.Core.Components.QuickEdit;

using ACDCs.Data.ACDCs.Components.Source;
using CircuitRenderer.Items;
using CircuitRenderer.Items.Sources;
using SpiceSharp.Components;

public class SourceEditorView : Grid
{
    private readonly SourceGridRow<double>? _amplitudeAcEntry;
    private readonly SourceGridRow<double>? _delayAcEntry;
    private readonly SourceGridRow<double>? _fallTimePulseEntry;
    private readonly SourceGridRow<double>? _frequencyAcEntry;
    private readonly SourceGridRow<double>? _initialValuePulseEntry;
    private readonly SourceGridRow<double>? _offsetAcEntry;
    private readonly SourceGridRow<double>? _periodPulseEntry;
    private readonly SourceGridRow<double>? _phaseAcEntry;
    private readonly SourceGridRow<double>? _pulsedValuePulseEntry;
    private readonly SourceGridRow<double>? _pulseWidthPulseEntry;
    private readonly SourceGridRow<double>? _riseTimePulseEntry;
    private readonly SourceGridRow<bool>? _selectorSourceType;
    private readonly SourceGridRow<double>? _thetaAcEntry;
    private readonly SourceGridRow<double>? _voltageAcEntry;
    private readonly SourceGridRow<double>? _voltageDcEntry;
    private readonly SourceGridRow<string>? _waveformAcSelect;
    private VoltageSourceItem? _source;
    public Action<WorksheetItem>? OnSourceEdited { get; set; }

    public SourceEditorView()
    {
        RowDefinition[] rows = { new(32), };

        ColumnDefinition[] columns = { new(GridLength.Star) };

        this.ColumnDefinitions(columns)
            .RowDefinitions(rows)
            .Margin(2)
            .Padding(2);

        _selectorSourceType = AddInput<bool>(
            description: "AC/DC",
            symbol: "DC",
            function: SwitchSourceType) as SourceGridRow<bool>;

        _voltageDcEntry = AddInput<double>("DC Voltage", symbol: "V - DC Voltage") as SourceGridRow<double>;

        List<string> waveFormList = new()
        {
            "Sine",
        //    "PWL",
            "Pulse"
        };

        _voltageAcEntry =
            AddInput<double>(description: "AC Voltage", symbol: "V", function: OnValueEdited) as SourceGridRow<double>;
        _waveformAcSelect = AddInput<string>("AC Waveform", symbol: "", options: waveFormList, function: OnWaveformSelect) as SourceGridRow<string>;
        _offsetAcEntry = AddInput<double>("Offset", symbol: "V", function: OnValueEdited) as SourceGridRow<double>;
        _amplitudeAcEntry = AddInput<double>("Amplitude", symbol: "V", function: OnValueEdited) as SourceGridRow<double>;
        _frequencyAcEntry = AddInput<double>("Frequency", symbol: "hz", function: OnValueEdited) as SourceGridRow<double>;
        _delayAcEntry = AddInput<double>("Delay", symbol: "Degrees", function: OnValueEdited) as SourceGridRow<double>;
        _thetaAcEntry = AddInput<double>("Theta", symbol: "Degrees", function: OnValueEdited) as SourceGridRow<double>;
        _phaseAcEntry = AddInput<double>("Phase", symbol: "Degrees", function: OnValueEdited) as SourceGridRow<double>;
        _initialValuePulseEntry = AddInput<double>("Initial Pulse value", symbol: "V", function: OnValueEdited) as SourceGridRow<double>;
        _pulsedValuePulseEntry = AddInput<double>("Pulsed value", symbol: "V", function: OnValueEdited) as SourceGridRow<double>;
        _riseTimePulseEntry = AddInput<double>("Rise time", symbol: "s", function: OnValueEdited) as SourceGridRow<double>;
        _fallTimePulseEntry = AddInput<double>("Fall time", symbol: "s", function: OnValueEdited) as SourceGridRow<double>;
        _pulseWidthPulseEntry = AddInput<double>("Pulse width", symbol: "s", function: OnValueEdited) as SourceGridRow<double>;
        _periodPulseEntry = AddInput<double>("Period", symbol: "s", function: OnValueEdited) as SourceGridRow<double>;

        RowDefinitions.Add(new RowDefinition(GridLength.Star));
        SwitchSourceType(this, false);
    }

    public void SetSource(VoltageSourceItem source)
    {
        _source = source;
        if (source.Model is not Source model)
            return;

        if (_selectorSourceType?.View is not Switch sw)
            return;

        if (string.IsNullOrEmpty(model.Type))
            model.Type = "DC";

        sw.IsToggled = model.Type == "AC";

        SetRowValue(_voltageDcEntry, model.DcValue);
        if (model.Waveform is Sine sine)
        {
            SetRowValue(_voltageAcEntry, model.AcValue);
            SetRowValue(_offsetAcEntry, sine.Offset);
            SetRowValue(_amplitudeAcEntry, sine.Amplitude);
            SetRowValue(_frequencyAcEntry, sine.Frequency);
            SetRowValue(_delayAcEntry, sine.Delay);
            SetRowValue(_thetaAcEntry, sine.Theta);
            SetRowValue(_phaseAcEntry, sine.Phase);
        }
        else if (model.Waveform is Pulse pulse)
        {
            SetRowValue(_initialValuePulseEntry, pulse.InitialValue);
            SetRowValue(_pulsedValuePulseEntry, pulse.PulsedValue);
            SetRowValue(_riseTimePulseEntry, pulse.RiseTime);
            SetRowValue(_fallTimePulseEntry, pulse.FallTime);
            SetRowValue(_pulseWidthPulseEntry, pulse.PulseWidth);
            SetRowValue(_periodPulseEntry, pulse.Period);
            SetRowValue(_delayAcEntry, pulse.Delay);
        }

        SwitchSourceType(this, model.Type == "AC");
    }

    private static void Enable(View? view, bool b)
    {
        if (view == null) return;
        view.IsVisible = b;
    }

    private static double GetRowValue(SourceGridRow<double> sourceGridRow)
    {
        if (sourceGridRow.View is Entry entry)
        {
            double.TryParse(entry.Text, out double value);
            return value;
        }

        return 0;
    }

    private static void SetRowValue(SourceGridRow<double>? sourceGridRow, double value)
    {
        if (sourceGridRow?.View is not Entry entry)
            return;
        entry.Text = value.ToString();
    }

    private View AddInput<T>(string description, string symbol = "", List<string>? options = null,
                    Action<object?, object?>? function = null)
    {
        Grid lineGrid = new SourceGridRow<T>(description, symbol, function, options);
        int rowNumber = RowDefinitions.Count;

        this.Add(lineGrid, 0, rowNumber);
        RowDefinitions.Add(new RowDefinition(GridLength.Auto));

        return lineGrid;
    }

    private void OnValueEdited(object? source, object? value)
    {
        if (source is not SourceGridRow<double> row) return;
        if (value is not TextChangedEventArgs args) return;
        if (args.OldTextValue == args.NewTextValue) return;
        double doubleValue = GetRowValue(row);
        if (_source?.Model is not Source sourceModel)
            return;

        if (row == _voltageAcEntry)
            sourceModel.AcValue = doubleValue;
        else if (row == _offsetAcEntry)
            Sine(sourceModel).Offset = doubleValue;
        else if (row == _amplitudeAcEntry)
            Sine(sourceModel).Amplitude = doubleValue;
        else if (row == _frequencyAcEntry)
            Sine(sourceModel).Frequency = doubleValue;
        else if (row == _delayAcEntry)
            Sine(sourceModel).Delay = doubleValue;
        else if (row == _thetaAcEntry)
            Sine(sourceModel).Theta = doubleValue;
        else if (row == _phaseAcEntry)
            Sine(sourceModel).Phase = doubleValue;
        else if (row == _initialValuePulseEntry)
            Pulse(sourceModel).InitialValue = doubleValue;
        else if (row == _pulsedValuePulseEntry)
            Pulse(sourceModel).PulsedValue = doubleValue;
        else if (row == _riseTimePulseEntry)
            Pulse(sourceModel).RiseTime = doubleValue;
        else if (row == _fallTimePulseEntry)
            Pulse(sourceModel).FallTime = doubleValue;
        else if (row == _pulseWidthPulseEntry)
            Pulse(sourceModel).PulseWidth = doubleValue;
        else if (row == _periodPulseEntry)
            Pulse(sourceModel).Period = doubleValue;
        else if (row == _delayAcEntry)
            Pulse(sourceModel).Delay = doubleValue;
    }

    private void OnWaveformSelect(object? arg1, object? arg2)
    {
        if (_selectorSourceType?.View is not Switch sw) return;
        SwitchSourceType(arg1, sw.IsToggled);
    }

    private Pulse Pulse(Source model)
    {
        if (model.Waveform is not Pulse pulse)
        {
            model.Waveform = pulse = new Pulse();
        }
        return pulse;
    }

    private Sine Sine(Source model)
    {
        if (model.Waveform is not Sine sine)
        {
            model.Waveform = sine = new Sine();
        }
        return sine;
    }

    private void SwitchSourceType(object? sender, object? value)
    {
        if (value is not bool acSelected)
            return;

        if (_source?.Model is not Source sourceModel)
            return;

        if (_waveformAcSelect is not { View: Picker picker })
            return;

        _selectorSourceType.SymbolLabel.Text = acSelected ? "AC" : "DC";

        sourceModel.Type = acSelected ? "AC" : "DC";

        bool isSine = picker.SelectedIndex == 0;
        bool isPulse = picker.SelectedIndex == 1;

        Enable(_voltageDcEntry, !acSelected);
        Enable(_voltageAcEntry, acSelected);
        Enable(_waveformAcSelect, acSelected);
        Enable(_offsetAcEntry, acSelected && isSine);
        Enable(_amplitudeAcEntry, acSelected && isSine);
        Enable(_frequencyAcEntry, acSelected && isSine);
        Enable(_delayAcEntry, acSelected && isSine);
        Enable(_thetaAcEntry, acSelected && isSine);
        Enable(_phaseAcEntry, acSelected && isSine);
        Enable(_initialValuePulseEntry, acSelected && isPulse);
        Enable(_pulsedValuePulseEntry, acSelected && isPulse);
        Enable(_riseTimePulseEntry, acSelected && isPulse);
        Enable(_fallTimePulseEntry, acSelected && isPulse);
        Enable(_pulseWidthPulseEntry, acSelected && isPulse);
        Enable(_periodPulseEntry, acSelected && isPulse);

        if (_source == null)
        {
            return;
        }

        if (_source.Model is not Source source)
        {
            return;
        }

        try
        {
            Entry? sourceDcValue = _voltageDcEntry?.View as Entry;
            source.DcValue = Convert.ToDouble(sourceDcValue?.Text);
            source.AcMagnitude = 0;
            source.AcPhase = 0;
            source.Phasor.Phase = 0;

            double offset = GetRowValue(_offsetAcEntry);
            double amplitude = GetRowValue(_amplitudeAcEntry);
            double frequency = GetRowValue(_frequencyAcEntry);
            double delay = GetRowValue(_delayAcEntry);
            double theta = GetRowValue(_thetaAcEntry);
            double phase = GetRowValue(_phaseAcEntry);
            double initialValue = GetRowValue(_initialValuePulseEntry);
            double pulsedValue = GetRowValue(_pulsedValuePulseEntry);
            double riseTime = GetRowValue(_riseTimePulseEntry);
            double fallTime = GetRowValue(_fallTimePulseEntry);
            double pulseWidth = GetRowValue(_pulseWidthPulseEntry);
            double period = GetRowValue(_periodPulseEntry);

            switch (picker.SelectedItem as string)
            {
                case "Sine":
                    if (source.Waveform is not Sine sine)
                        source.Waveform = new Sine(offset, amplitude, frequency, delay, theta, phase);
                    else
                    {
                        sine.Offset = offset;
                        sine.Amplitude = amplitude;
                        sine.Frequency = frequency;
                        sine.Delay = delay;
                        sine.Theta = theta;
                        sine.Phase = phase;
                    }

                    break;

                case "PWL":
                    Pwl pwl;
                    source.Waveform = pwl = new Pwl();
                    pwl.SetPoints(Array.Empty<double>());
                    break;

                case "Pulse":
                    if (source.Waveform is not Pulse pulse)
                        source.Waveform = new Pulse(initialValue, pulsedValue, delay, riseTime, fallTime, pulseWidth, period);
                    else
                    {
                        pulse.InitialValue = initialValue;
                        pulse.PulsedValue = pulsedValue;
                        pulse.RiseTime = riseTime;
                        pulse.FallTime = fallTime;
                        pulse.PulseWidth = pulseWidth;
                        pulse.Period = period;
                        pulse.Delay = delay;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
        }
    }
}
