namespace ACDCs.Components.Properties;

using Sharp.UI;

public class PropertyTemplate : ContentView
{
    private Action<PropertyEditorView>? _onModelEditorClicked;
    private Action<PropertyEditorView>? _onModelSelectionClicked;
    private Action<string, object>? _updatePropertyAction;

    public PropertyTemplate(Action<string, object> updatePropertyAction,
        Action<PropertyEditorView> onModelSelectionClicked, Action<PropertyEditorView> onModelEditorClicked)
    {
        _updatePropertyAction = updatePropertyAction;
        _onModelSelectionClicked = onModelSelectionClicked;
        _onModelEditorClicked = onModelEditorClicked;
        Grid grid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Start)
            .Margin(1)
            .Padding(0)
            .ColumnSpacing(0)
            .RowSpacing(0);

        grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(new GridLength(60)));
        grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star));
        grid.RowDefinitions.Add(new Microsoft.Maui.Controls.RowDefinition(GridLength.Auto));
        Label label = new Label()
            .Margin(1).Padding(1);

        label.SetBinding(Label.TextProperty, "Name");
        grid.Add(label);

        PropertyEditorView entry = new PropertyEditorView()
            .HorizontalOptions(LayoutOptions.Fill)
            .Margin(1)
            .Padding(1);

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

    ~PropertyTemplate()
    {
        Content = null;
        _onModelEditorClicked = null;
        _onModelSelectionClicked = null;
        _updatePropertyAction = null;
    }

    private void UpdateProperty(string entryPropertyName, object newValue)
    {
        _updatePropertyAction?.Invoke(entryPropertyName, newValue);
    }
}
