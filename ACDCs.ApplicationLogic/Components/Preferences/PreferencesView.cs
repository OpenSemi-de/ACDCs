namespace ACDCs.API.Core.Components.Preferences;

using Instance;
using IO.DB;
using Newtonsoft.Json;
using Properties;

public class PreferencesView : Grid
{
    private static List<PreferenceSetting>? s_preferences;
    private StackLayout? _preferencesLayout;
    private PreferencesRepository _repository = new();

    public PreferencesView()
    {
        Loaded += OnLoaded;
    }

    private static async Task<List<PreferenceSetting>> GetPreferenceTemplate()
    {
        string jsonData = await API.LoadMauiAssetAsString("preferences.json");
        List<PreferenceSetting>? items = JsonConvert.DeserializeObject<List<PreferenceSetting>>(jsonData);
        return items ?? new List<PreferenceSetting>();
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        s_preferences ??= await GetPreferenceTemplate();

        ColumnDefinitionCollection columns = new()
        {
            new ColumnDefinition(GridLength.Star),
            new ColumnDefinition(new GridLength(120))
        };

        RowDefinitionCollection rows = new()
        {
            new RowDefinition(GridLength.Star),
            new RowDefinition(new GridLength(34))
        };

        this.HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .ColumnDefinitions(columns)
            .RowDefinitions(rows)
            .Padding(0)
            .Margin(0);

        _preferencesLayout = new StackLayout()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        Add(_preferencesLayout);

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
