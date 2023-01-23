using ACDCs.Views.Properties;

namespace ACDCs.Components.Properties;

using Sharp.UI;

public class PropertyTemplate : ContentView
{
    private readonly Action<PropertyEditorView> _onModelEditorClicked;
    private readonly Action<PropertyEditorView> _onModelSelectionClicked;
    private readonly Action<string, object> _updatePropertyAction;

    public PropertyTemplate(Action<string, object> updatePropertyAction,
        Action<PropertyEditorView> onModelSelectionClicked, Action<PropertyEditorView> onModelEditorClicked)
    {
        _updatePropertyAction = updatePropertyAction;
        _onModelSelectionClicked = onModelSelectionClicked;
        _onModelEditorClicked = onModelEditorClicked;
        Grid grid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Start);

        grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(new GridLength(80)));
        grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star));
        grid.RowDefinitions.Add(new Microsoft.Maui.Controls.RowDefinition(GridLength.Auto));
        Label label = new Label()
            .FontSize(11);

        label.SetBinding(Label.TextProperty, "Name");
        grid.Add(label);

        PropertyEditorView entry = new PropertyEditorView()
            .HorizontalOptions(LayoutOptions.Fill)
            .Fontsize(11);
        entry.SetBinding(PropertyEditorView.PropertyNameProperty, "Name", BindingMode.OneTime);
        entry.SetBinding(PropertyEditorView.ValueProperty, "Value", BindingMode.OneTime);

        entry.OnModelSelectionClicked = _onModelSelectionClicked;
        entry.OnModelEditorClicked = _onModelEditorClicked;
        entry.OnValueChanged += newValue =>
        {
            UpdateProperty(entry.PropertyName, newValue);
        };

        grid.Add(entry);
        Grid.SetColumn(entry, 1);
        Content = grid;
    }

    private void UpdateProperty(string entryPropertyName, object newValue)
    {
        _updatePropertyAction.Invoke(entryPropertyName, newValue);
    }
}
