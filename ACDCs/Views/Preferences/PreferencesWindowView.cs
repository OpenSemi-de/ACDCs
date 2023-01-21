using ACDCs.Components;
using ACDCs.Components.Window;
using ACDCs.IO.DB;
using ACDCs.Views.Properties;

namespace ACDCs.Views.Preferences;

using Sharp.UI;

public class PreferencesWindowView : WindowView
{
    public static List<IPreferenceSetting> preference = new()
    {
        new PreferenceSetting<bool>(false, "DarkMode"),
    };

    private readonly Grid _layoutGrid;
    private readonly StackLayout _preferencesLayout;

    public PreferencesWindowView(SharpAbsoluteLayout layout) : base(layout, "Preferences")
    {
        ColumnDefinitionCollection columns = new()
        {
            new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star),
            new Microsoft.Maui.Controls.ColumnDefinition(new GridLength(120))
        };

        RowDefinitionCollection rows = new()
        {
            new Microsoft.Maui.Controls.RowDefinition(GridLength.Star),
            new Microsoft.Maui.Controls.RowDefinition(new GridLength(34))
        };

        _layoutGrid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .ColumnDefinitions(columns)
            .RowDefinitions(rows)
            .Padding(0)
            .Margin(0);

        _preferencesLayout = new StackLayout()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _layoutGrid.Add(_preferencesLayout);

        foreach (IPreferenceSetting preferenceSetting in preference)
        {
            PropertyEditorView propertyEditorView = new PropertyEditorView();
            propertyEditorView.PropertyName = preferenceSetting.Key;
            propertyEditorView.Value = preferenceSetting.ObjectValue;
            _preferencesLayout.Add(propertyEditorView);
        }

        WindowContent = _layoutGrid;
    }
}
