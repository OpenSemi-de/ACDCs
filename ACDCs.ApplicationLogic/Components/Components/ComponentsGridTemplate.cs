namespace ACDCs.API.Core.Components.Components;

using Sharp.UI;
using ColumnDefinition = ColumnDefinition;
using RowDefinition = RowDefinition;

public class ComponentsGridTemplate : DataTemplate
{
    public ComponentsGridTemplate()
    {
        Add(() =>
        {
            ViewCell viewCell = new() { GetView() };
            return viewCell;
        });
    }

    private static Grid GetView()
    {
        RowDefinitionCollection rowDefinitions = new()
        {
            new RowDefinition(32)
        };

        ColumnDefinitionCollection columnDefinitions = new()
        {
            new ColumnDefinition(100),
            new ColumnDefinition(100),
            new ColumnDefinition(),
            new ColumnDefinition(60),
            new ColumnDefinition(60),
            new ColumnDefinition(60)
        };

        Label nameLabel = new Label()
            .FontAttributes(FontAttributes.Bold)
            .Bind(Label.TextProperty, "Name");
        Label typeLabel = new Label()
            .Bind(Label.TextProperty, "Type")
            .Column(1);
        Label valueLabel = new Label()
            .Bind(Label.TextProperty, "Value")
            .Column(2);

        Grid grid = new Grid
            {
                nameLabel,
                typeLabel,
                valueLabel
            }
            .RowDefinitions(rowDefinitions)
            .ColumnDefinitions(columnDefinitions);
        return grid;
    }
}
