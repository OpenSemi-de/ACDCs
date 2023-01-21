using ACDCs.Components;
using ACDCs.Components.Window;
using ACDCs.IO.DB;
using ACDCs.Views.Properties;

namespace ACDCs.Views.Preferences;

using Sharp.UI;

public class PreferencesWindowView : WindowView
{
    public static List<PreferenceSetting> preferences = new()
    {
        new PreferenceSetting {Key = "DarkMode", Value = false},
        new PreferenceSetting {Key = "StartDebugConsole", Value = false}
    };

    private readonly Grid _layoutGrid;
    private readonly StackLayout _preferencesLayout;
    private readonly PreferencesRepository _repository = new();

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

        foreach (PreferenceSetting preferenceSetting in preferences)
        {
            object? loadedPreference = _repository?.GetPreference(preferenceSetting.Key);
            StackLayout horizontaLayout = new StackLayout()
                .HorizontalOptions(LayoutOptions.Fill)
                .Orientation(StackOrientation.Horizontal);

            Label propertyLabel = new Label(preferenceSetting.Key)
                .WidthRequest(100);

            PropertyEditorView propertyEditorView = new()
            {
                PropertyName = preferenceSetting.Key,
                Value = loadedPreference ?? preferenceSetting.Value,
                OnValueChanged = o => OnValueChanged(preferenceSetting.Key, o)
            };

            horizontaLayout.Add(propertyLabel);
            horizontaLayout.Add(propertyEditorView);
            _preferencesLayout.Add(horizontaLayout);
        }

        _repository = new PreferencesRepository();

        WindowContent = _layoutGrid;
    }

    private void OnValueChanged(string key, object obj)
    {
        switch (obj)
        {
            case bool b:
                _repository.SetPreference(key, b);
                break;

            case string s:
                _repository.SetPreference(key, s);
                break;
        }
    }
}
