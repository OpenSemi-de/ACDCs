using ACDCs.Components.Window;
using ACDCs.Views.Properties;
using Sharp.UI;
using ContentView = Sharp.UI.ContentView;
using Grid = Sharp.UI.Grid;
using Label = Sharp.UI.Label;
using PropertyEditorView = ACDCs.Components.Properties.PropertyEditorView;

namespace ACDCs.Components.ModelEditor;

public class ModelPropertyTemplate : ContentView
{
    private readonly Action<string, object> _updatePropertyAction;

    public ModelPropertyTemplate(IGetPropertyUpdates window)
    {
        _updatePropertyAction = window.OnPropertyUpdated;
        Grid grid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Start);

        grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(new GridLength(100)));
        grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star));
        grid.RowDefinitions.Add(new Microsoft.Maui.Controls.RowDefinition(new GridLength(34)));
        Label label = new Label()
            .FontSize(11);

        label.SetBinding(Label.TextProperty, "Name");
        grid.Add(label);

        PropertyEditorView entry = new PropertyEditorView(window as WindowView)
            .HorizontalOptions(LayoutOptions.Fill);
        entry.ShowDescription = true;
        entry.SetBinding(PropertyEditorView.ParentTypeProperty, "ParentType", BindingMode.OneTime);
        entry.SetBinding(PropertyEditorView.PropertyNameProperty, "Name", BindingMode.OneTime);
        entry.SetBinding(PropertyEditorView.ValueProperty, "Value", BindingMode.OneTime);

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
