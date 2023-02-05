namespace ACDCs.ApplicationLogic.Views.Preferences;

using ACDCs.ApplicationLogic.Components.Properties;
using ACDCs.ApplicationLogic.Components.Window;
using IO.DB;
using Newtonsoft.Json;
using Sharp.UI;

public class PreferencesWindow : Window
{
    private static List<PreferenceSetting>? s_preferences;
    private Grid? _layoutGrid;
    private StackLayout? _preferencesLayout;
    private PreferencesRepository _repository = new();

    public static PreferencesWindow? Instance { get; set; }

    // ReSharper disable once UnusedMember.Global
    public PreferencesWindow(WindowContainer? layout) : base(layout, "Preferences")
    {
        Initialize();
    }

    private static async Task<List<PreferenceSetting>> GetPreferenceTemplate()
    {
        string jsonData = await API.LoadMauiAssetAsString("preferences.json");
        List<PreferenceSetting>? items = JsonConvert.DeserializeObject<List<PreferenceSetting>>(jsonData);
        return items ?? new List<PreferenceSetting>();
    }

    private void Initialize()
    {
        Instance = this;
        Loaded += OnLoaded;
        OnClose = () =>
        {
            Instance = null;
            return false;
        };
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        s_preferences ??= await GetPreferenceTemplate();

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

        foreach (PreferenceSetting preferenceSetting in s_preferences.OrderBy(preference => preference.Group))
        {
            if (preferenceSetting.Key == null)
            {
                continue;
            }

            object? loadedPreference = _repository?.GetPreference(preferenceSetting.Key);
            StackLayout horizontaLayout = new StackLayout()
                .HorizontalOptions(LayoutOptions.Fill)
                .Orientation(StackOrientation.Horizontal);

            Label groupLabel = new Label(preferenceSetting.Group).WidthRequest(60);
            Label keyLabel = new Label(preferenceSetting.Key).WidthRequest(80);
            Label propertyLabel = new Label(preferenceSetting.Description).WidthRequest(140);

            PropertyEditorView propertyEditorView = new()
            {
                PropertyName = preferenceSetting.Key,
                Value = loadedPreference ?? preferenceSetting.Value,
                OnValueChanged = o => OnValueChanged(preferenceSetting.Key, o)
            };

            horizontaLayout.Add(groupLabel);
            horizontaLayout.Add(keyLabel);
            horizontaLayout.Add(propertyLabel);
            horizontaLayout.Add(propertyEditorView);
            _preferencesLayout.Add(horizontaLayout);
        }

        _repository = new PreferencesRepository();
        Start();
        ChildLayout.Add(_layoutGrid);
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
