namespace ACDCs.Components.ModelSelection;

using Sharp.UI;

public class ModelSelectionCell : ViewCell
{
    private readonly Grid _grid;
    private readonly Label _labelName;
    private readonly Label _labelType;
    private readonly Label _labelValue;

    public ModelSelectionCell()
    {
        _grid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .HeightRequest(34);

        _grid.SetBinding(VisualElement.BackgroundColorProperty, "ItemBackground");

        _grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(new GridLength(80)));
        _grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(new GridLength(80)));
        _grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star));

        _labelName = new Label()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _labelName.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, "Name");
        _grid.Add(_labelName);

        _labelType = new Label()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);
        Microsoft.Maui.Controls.Grid.SetColumn(_labelType, 1);

        _labelType.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, "Type");
        _grid.Add(_labelType);

        _labelValue = new Label()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);
        Microsoft.Maui.Controls.Grid.SetColumn(_labelValue, 2);

        _labelValue.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, "Value");
        _grid.Add(_labelValue);

        Add(_grid);
    }
}
