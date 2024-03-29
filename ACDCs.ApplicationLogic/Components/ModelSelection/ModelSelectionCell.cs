﻿namespace ACDCs.API.Core.Components.ModelSelection;

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

        _grid.ColumnDefinitions(new ColumnDefinition(new GridLength(80)), new ColumnDefinition(new GridLength(80)), new ColumnDefinition(GridLength.Star));

        _labelName = new Label()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _labelName.SetBinding(Label.TextProperty, "Name");
        _grid.Add(_labelName);

        _labelType = new Label()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);
        Grid.SetColumn(_labelType, 1);

        _labelType.SetBinding(Label.TextProperty, "Type");
        _grid.Add(_labelType);

        _labelValue = new Label()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);
        Grid.SetColumn(_labelValue, 2);

        _labelValue.SetBinding(Label.TextProperty, "Value");
        _grid.Add(_labelValue);

        Add(_grid);
    }
}
