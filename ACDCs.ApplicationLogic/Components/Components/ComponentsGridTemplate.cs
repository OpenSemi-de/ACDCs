namespace ACDCs.ApplicationLogic.Components.Components;

using Sharp.UI;

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
            new Microsoft.Maui.Controls.RowDefinition(32)
        };

        ColumnDefinitionCollection columnDefinitions = new()
        {
            new Microsoft.Maui.Controls.ColumnDefinition(100),
            new Microsoft.Maui.Controls.ColumnDefinition(100),
            new Microsoft.Maui.Controls.ColumnDefinition(),
            new Microsoft.Maui.Controls.ColumnDefinition(60),
            new Microsoft.Maui.Controls.ColumnDefinition(60),
            new Microsoft.Maui.Controls.ColumnDefinition(60)
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

        Grid grid = new Grid()
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
