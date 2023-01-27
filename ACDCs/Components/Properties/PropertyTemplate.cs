using ACDCs.Components.Window;
using ACDCs.Views.Properties;

namespace ACDCs.Components.Properties;

using Sharp.UI;

public class PropertyTemplate : ContentView
{
    private PropertyEditorView? _entry;
    private Grid? _grid;
    private Label? _label;
    private Action<string, object>? _updatePropertyAction;

    public PropertyTemplate(IGetPropertyUpdates getPropertyUpdates)
    {
        _updatePropertyAction = getPropertyUpdates.OnPropertyUpdated;
        _grid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Start)
            .Margin(1)
            .Padding(0)
            .ColumnSpacing(0)
            .RowSpacing(0);

        _grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(new GridLength(60)));
        _grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star));
        _grid.RowDefinitions.Add(new Microsoft.Maui.Controls.RowDefinition(GridLength.Auto));
        _label = new Label()
            .Margin(1).Padding(1);

        _label.SetBinding(Label.TextProperty, "Name");
        _grid.Add(_label);

        _entry = new PropertyEditorView(getPropertyUpdates as WindowView)
            .HorizontalOptions(LayoutOptions.Fill)
            .Margin(1)
            .Padding(1);

        _entry.SetBinding(PropertyEditorView.PropertyNameProperty, "Name", BindingMode.OneTime);
        _entry.SetBinding(PropertyEditorView.ValueProperty, "Value", BindingMode.OneTime);
        _entry.OnValueChanged += newValue =>
                {
                    UpdateProperty(_entry.PropertyName, newValue);
                };

        _grid.Add(_entry);
        Grid.SetColumn(_entry, 1);
        Content = _grid;
        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        Unloaded -= OnUnloaded;
        _label?.RemoveBinding(Label.TextProperty);
        _entry?.RemoveBinding(PropertyEditorView.PropertyNameProperty);
        _entry?.RemoveBinding(PropertyEditorView.ValueProperty);
        _updatePropertyAction = null;
        if (_entry != null)
        {
            _entry.OnValueChanged = null;
        }

        _grid?.Remove(_entry);
        _grid?.Remove(_label);

        _entry = null;
        _label = null;
        _grid = null;

        BindingContext = null;
    }

    private void UpdateProperty(string entryPropertyName, object newValue)
    {
        _updatePropertyAction?.Invoke(entryPropertyName, newValue);
    }
}
