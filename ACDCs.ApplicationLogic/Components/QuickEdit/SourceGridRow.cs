namespace ACDCs.API.Core.Components.QuickEdit;

public class SourceGridRow<T> : Grid
{
    public Label DescriptionLabel { get; }

    public Label SymbolLabel { get; }

    public View? View { get; }

    public SourceGridRow(string description, string symbol, Action<object?, object?> function, List<string> options)
    {
        this.ColumnDefinitions(
            new ColumnDefinition(70),
            new ColumnDefinition(70),
            new ColumnDefinition(20));

        RowDefinitions.Add(new RowDefinition(32));

        DescriptionLabel = new Label(description)
            .VerticalTextAlignment(TextAlignment.Center);

        if (typeof(T) == typeof(string) ||
            typeof(T) == typeof(double))
        {
            View = new Entry();
            this.Add(View, 1);
        }
        else if (typeof(T) == typeof(bool))
        {
            Switch? sw = new();
            sw.OnToggled((sender, e) => function?.Invoke(sender, e.Value));
            View = sw;
            this.Add(sw, 1);
        }

        SymbolLabel = new Label(symbol)
            .VerticalTextAlignment(TextAlignment.Center);

        this.Add(DescriptionLabel);
        this.Add(SymbolLabel, 2);
    }
}
