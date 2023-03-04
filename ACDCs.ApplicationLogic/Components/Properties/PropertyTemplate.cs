namespace ACDCs.API.Core.Components.Properties;

using ACDCs.API.Windowing.Components.Window;
using Interfaces;
using Sharp.UI;
using ColumnDefinition = ColumnDefinition;
using RowDefinition = RowDefinition;

public class PropertyTemplate : Grid
{
    private readonly PropertyEditorView? _entry;
    private readonly Label? _label;
    private Action<string, object>? _updatePropertyAction;

    public PropertyTemplate(IGetPropertyUpdates getPropertyUpdates)
    {
        _updatePropertyAction = getPropertyUpdates.OnPropertyUpdated;
        this.HorizontalOptions(LayoutOptions.Fill);

        ColumnDefinitions.Add(new ColumnDefinition(new GridLength(60)));
        ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        _label = new Label();

        _label.SetBinding(Label.TextProperty, "Name");
        Add(_label);

        _entry = new PropertyEditorView(getPropertyUpdates as Window)
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
