namespace ACDCs.API.Core.Components.QuickEdit;

public class SourceGridRow<T> : Grid
{
    private bool _isEnabled = true;

    public Label DescriptionLabel { get; }

    public new bool IsEnabled
    {
        get { return _isEnabled; }
        set
        {
            this.IsVisible(value);
            this.View.IsEnabled = value;
            this.DescriptionLabel.IsEnabled = value;
            this.SymbolLabel.IsEnabled = value;
            _isEnabled = value;
        }
    }

    public Label SymbolLabel { get; }

    public View? View { get; }

    public SourceGridRow(string description, string symbol, Action<object?, object?>? function, List<string>? options)
    {
        this.ColumnDefinitions(
            new ColumnDefinition(100),
            new ColumnDefinition(120),
            new ColumnDefinition(GridLength.Star));

        RowDefinitions.Add(new RowDefinition(32));

        DescriptionLabel = new Label(description)
            .VerticalTextAlignment(TextAlignment.Center)
            .HorizontalOptions(LayoutOptions.End);

        if (options != null)
        {
            Picker picker = new Picker()
                .OnSelectedIndexChanged((sender, value) =>
                {
                    function?.Invoke(this, value);
                });

            picker.ItemsSource = options;
            picker.SelectedIndex = 0;
            this.Add(picker, 1);
            View = picker;
        }
        else if (typeof(T) == typeof(string) ||
            typeof(T) == typeof(double))
        {
            Entry entry = new Entry()
                .OnTextChanged((sender, value) =>
                {
                    function?.Invoke(this, value);
                });

            this.Add(entry, 1);
            View = entry;
        }
        else if (typeof(T) == typeof(bool))
        {
            Switch? sw = new();
            sw.OnToggled((sender, e) => function?.Invoke(sender, e.Value));
            View = sw;
            this.Add(sw, 1);
        }

        SymbolLabel = new Label(symbol)
            .VerticalTextAlignment(TextAlignment.Center)
            .HorizontalOptions(LayoutOptions.Start);

        Add(DescriptionLabel);
        this.Add(SymbolLabel, 2);
    }
}
