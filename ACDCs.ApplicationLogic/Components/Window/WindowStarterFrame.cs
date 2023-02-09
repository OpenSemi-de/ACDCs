namespace ACDCs.ApplicationLogic.Components.Window;

using Newtonsoft.Json;
using Sharp.UI;

public class WindowStarterFrame : Frame
{
    private readonly Label _badge;
    private readonly StackLayout _badgeBg;
    private readonly StackLayout _buttonStack;
    private readonly Grid _grid;
    private readonly ScrollView _scrollView;
    private readonly WindowContainer? _windowContainer;
    private bool _loaded;

    public WindowStarterFrame(WindowContainer? windowContainer = null)
    {
        _windowContainer = windowContainer;
        ColumnDefinitionCollection columns = new()
        {
            new ColumnDefinition(20),
            new ColumnDefinition()
        };

        RowDefinitionCollection rows = new()
        {
            new RowDefinition()
        };

        _grid = new Grid()
            .HeightRequest(400)
            .WidthRequest(200)
            .ColumnDefinitions(columns)
            .RowDefinitions(rows);

        _badgeBg = new StackLayout().BackgroundColor(API.Instance.Foreground)

            .Row(0).Column(0);
        _grid.Add(_badgeBg);

        _badge = new Label("").Rotation(-90)
            .HorizontalOptions(LayoutOptions.Start).VerticalOptions(LayoutOptions.Start)
            .FontSize(11);

        _badgeBg.Add(_badge);

        _scrollView = new ScrollView()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .Row(0).Column(1);

        _grid.Add(_scrollView);

        _buttonStack = new StackLayout()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _scrollView.Content = _buttonStack;

        BorderColor = API.Instance.Border;
        BackgroundColor = API.Instance.BackgroundHigh;
        WidthRequest = 200;
        Padding = 0;
        Margin = 0;
        Content = _grid;
        this.ZIndex(1000);
        API.Reset += API_Reset;
        Opacity = 0;
    }

    public void FadeIn()
    {
        IsVisible = true;
        this.FadeTo(1, 500);
    }

    public async void Load()
    {
        if (_loaded) return;
        string json = await API.LoadMauiAssetAsString("starters.json");
        Dictionary<string, string> starters = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ??
                                            new Dictionary<string, string>();

        foreach (KeyValuePair<string, string> starter in starters)
        {
            Type? startType = GetType().Assembly.GetTypes().FirstOrDefault(t => t.Name == starter.Value);
            WindowStarterButton newButton = new(starter.Key, startType, _windowContainer);
            _buttonStack.Add(newButton);
        }
        _loaded = true;
    }

    private void API_Reset(object sender, ResetEventArgs args)
    {
        FadeOut();
    }

    private void FadeOut()
    {
        this.FadeTo(0, 500);
        Task.Delay(500);
        IsVisible = false;
    }

    /*
    private async void ShowComponents(object? sender, EventArgs e)
    {
        await API.Call(() =>
        {
            if (_componentsWindowView == null)
            {
                _componentsView = new ComponentsView();

                _componentsWindowView = new WindowView(API.MainContainer, "Components")
                {
                    WindowContent = _componentsView,
                    OnClose = OnCloseComponentsView
                };

                API.TabBar?.AddWindow(_componentsWindowView);
            }

            _componentsWindowView?.Maximize();
            return Task.CompletedTask;
        });

        FadeOut();
    }

    private async void ShowPreferences(object? sender, EventArgs e)
    {
        await API.Call(() =>
        {
            if (PreferencesWindowView.PreferencesWindow == null)
            {
                PreferencesWindowView.PreferencesWindow = new PreferencesWindowView();
                API.Open(PreferencesWindowView.PreferencesWindow);
            }
            else
            {
                API.TabBar?.BringToFront(PreferencesWindowView.PreferencesWindow);
            }

            return Task.CompletedTask;
        });

        FadeOut();
    }*/
}
