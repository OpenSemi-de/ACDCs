namespace ACDCs.API.Core.Components.QuickEdit;

using CircuitRenderer.Items;
using CircuitRenderer.Items.Sources;

public class SourceEditorView : Grid
{
    private readonly SourceGridRow<double>? _currentEntry;
    private readonly SourceGridRow<double>? _voltageEntry;
    private VoltageSourceItem? _source;
    public Action<WorksheetItem>? OnSourceEdited { get; set; }

    public SourceEditorView()
    {
        RowDefinition[] rows =
        {
            new(32),
        };

        ColumnDefinition[] columns =
        {
            new(GridLength.Star)
        };

        this.ColumnDefinitions(columns)
            .RowDefinitions(rows)
            .Margin(2)
            .Padding(2);

        AddInput<bool>("Voltage/current", options: new List<string> { "Voltage", "Current" }, function: SwitchSourceType);
        _voltageEntry = AddInput<double>("Voltage", "V") as SourceGridRow<double>;
        _currentEntry = AddInput<double>("Current", "A") as SourceGridRow<double>;
        this.RowDefinitions.Add(new(GridLength.Star));
    }

    public void SetSource(VoltageSourceItem source)
    {
        _source = source;
    }

    private View? AddInput<T>(string description, string symbol = "", Action<object?, object?>? function = null,
        List<string>? options = null)
    {
        View? input = null;
        Grid lineGrid = new SourceGridRow<T>(description, symbol, function, options);
        int rowNumber = RowDefinitions.Count;

        this.Add(lineGrid, 0, rowNumber);
        this.RowDefinitions.Add(new(32));

        return input;
    }

    private void SwitchSourceType(object? sender, object? value)
    {
        if (value is not bool currentSelected)
        {
            return;
        }

        if (_voltageEntry != null)
        {
            _voltageEntry.View.IsEnabled = !currentSelected;
        }

        if (_currentEntry != null)
        {
            _currentEntry.View.IsEnabled = currentSelected;
        }
    }
}
