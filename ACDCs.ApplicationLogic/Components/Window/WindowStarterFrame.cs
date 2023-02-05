namespace ACDCs.ApplicationLogic.Components.Window;

using Circuit;
using Sharp.UI;

public class WindowStarterFrame : Frame
{
    private readonly StackLayout _buttonStack;
    private readonly Grid _grid;
    private readonly ScrollView _scrollView;
    private readonly WindowContainer? _windowContainer;

    public WindowStarterFrame(WindowContainer? windowContainer = null)
    {
        _windowContainer = windowContainer;
        ColumnDefinitionCollection columns = new()
        {
            new ColumnDefinition()
        };

        RowDefinitionCollection rows = new()
        {
            new RowDefinition(20),
            new RowDefinition()
        };

        _grid = new Grid()
            .HeightRequest(400)
            .WidthRequest(200)
            .ColumnDefinitions(columns)
            .RowDefinitions(rows);

        _scrollView = new ScrollView()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _grid.Add(_scrollView);
        Grid.SetRow(_scrollView, 1);

        _buttonStack = new StackLayout()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _scrollView.Content = _buttonStack;

        WindowStarterButton newCircuit = new("New circuit window", typeof(CircuitWindow), windowContainer);
        _buttonStack.Add(newCircuit);

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
