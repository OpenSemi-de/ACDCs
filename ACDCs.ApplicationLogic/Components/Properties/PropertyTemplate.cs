using ACDCs.ApplicationLogic.Views.Properties;

namespace ACDCs.ApplicationLogic.Components.Properties;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

public class PropertyTemplate : Grid
{
    private readonly PropertyEditorView? _entry;
    private readonly Label? _label;
    private Action<string, object>? _updatePropertyAction;

    public PropertyTemplate(IGetPropertyUpdates getPropertyUpdates)
    {
        _updatePropertyAction = getPropertyUpdates.OnPropertyUpdated;
        this.HorizontalOptions(LayoutOptions.Fill);

        ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(new GridLength(60)));
        ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star));
        RowDefinitions.Add(new Microsoft.Maui.Controls.RowDefinition(GridLength.Auto));
        _label = new Label();

        _label.SetBinding(Label.TextProperty, "Name");
        Add(_label);

        _entry = new PropertyEditorView(getPropertyUpdates as Window.Window)
            .HorizontalOptions(LayoutOptions.Fill)
            .Margin(1)
            .Padding(1);

        _entry.SetBinding(PropertyEditorView.PropertyNameProperty, "Name", BindingMode.OneTime);
        _entry.SetBinding(PropertyEditorView.ValueProperty, "Value", BindingMode.OneTime);
        _entry.OnValueChanged += newValue =>
                {
                    UpdateProperty(_entry.PropertyName, newValue);
                };

        Add(_entry);
        Grid.SetColumn(_entry, 1);
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
    }

    private void UpdateProperty(string entryPropertyName, object newValue)
    {
        _updatePropertyAction?.Invoke(entryPropertyName, newValue);
    }
}
