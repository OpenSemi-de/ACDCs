using ACDCs.Views.Properties;

namespace ACDCs.Components.Properties;

using Sharp.UI;

public class PropertyTemplate : ViewCell
{
    private readonly Action<PropertyEditorView> _onModelSelectionClicked;
    private readonly Action<string, object> _updatePropertyAction;

    public PropertyTemplate(Action<string, object> updatePropertyAction, Action<PropertyEditorView> onModelSelectionClicked)
    {
        _updatePropertyAction = updatePropertyAction;
        _onModelSelectionClicked = onModelSelectionClicked;
        Grid grid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .HeightRequest(30);
        grid.ColumnDefinitions.Add(new(new(80)));
        grid.ColumnDefinitions.Add(new(GridLength.Star));
        grid.RowDefinitions.Add(new(new(30)));
        Label label = new Label()
            .FontSize(11);

        label.SetBinding(Label.TextProperty, "Name");
        grid.Add(label);

        PropertyEditorView entry = new PropertyEditorView()
            .HorizontalOptions(LayoutOptions.Fill)
            .HeightRequest(30)
            .Fontsize(11);
        entry.SetBinding(PropertyEditorView.PropertyNameProperty, "Name", BindingMode.OneTime);
        entry.SetBinding(PropertyEditorView.ValueProperty, "Value", BindingMode.OneTime);

        entry.OnModelSelectionClicked = _onModelSelectionClicked;

        entry.OnValueChanged += newValue =>
        {
            UpdateProperty(entry.PropertyName, newValue);
        };

        grid.Add(entry);
        Grid.SetColumn(entry, 1);
        Add(grid);
    }

    private void UpdateProperty(string entryPropertyName, object newValue)
    {
        _updatePropertyAction.Invoke(entryPropertyName, newValue);
    }
}
