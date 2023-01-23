using ACDCs.Services;
using ACDCs.Views;
using ACDCs.Views.Preferences;
using Sharp.UI;
using Button = Sharp.UI.Button;
using Frame = Sharp.UI.Frame;
using Grid = Sharp.UI.Grid;
using ScrollView = Sharp.UI.ScrollView;
using StackLayout = Sharp.UI.StackLayout;

namespace ACDCs.Components.Window;

public class WindowStarterFrame : Frame
{
    private readonly StackLayout _buttonStack;
    private readonly Grid _grid;
    private readonly ScrollView _scrollView;
    private int _circuitCount;
    private ComponentsView? _componentsView;
    private WindowView? _componentsWindowView;
    private PreferencesWindowView? _preferencesWindowView;

    public WindowStarterFrame()
    {
        ColumnDefinitionCollection columns = new()
        {
            new Microsoft.Maui.Controls.ColumnDefinition(new GridLength(28)),
            new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star)
        };

        RowDefinitionCollection rows = new()
        {
            new Microsoft.Maui.Controls.RowDefinition(new GridLength(20)),
            new Microsoft.Maui.Controls.RowDefinition(GridLength.Star)
        };

        _grid = new Grid()
            .HeightRequest(400)
            .WidthRequest(240)
            .ColumnDefinitions(columns)
            .RowDefinitions(rows);

        _scrollView = new ScrollView()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _grid.Add(_scrollView);
        Grid.SetRow(_scrollView, 1);
        Grid.SetColumn(_scrollView, 1);

        _buttonStack = new StackLayout()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _scrollView.Content = _buttonStack;

        Button newCircuit = new Button("New circuit")
            .OnClicked(NewCircuit);
        _buttonStack.Add(newCircuit);

        Button components = new Button("Components")
            .OnClicked(ShowComponents);
        _buttonStack.Add(components);

        Button preferences = new Button("Preferences")
            .OnClicked(ShowPreferences);
        _buttonStack.Add(preferences);

        BorderColor = ColorService.Border;
        BackgroundColor = ColorService.BackgroundHigh;
        WidthRequest = 240;
        Padding = 0;
        Margin = 0;
        Content = _grid;
        this.ZIndex(1000);
        API.Reset += API_Reset;
        Opacity = 0;
    }

    public void FadeIn()
    {
        this.IsVisible = true;
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
        this.IsVisible = false;
    }

    private async void NewCircuit(object? sender, EventArgs e)

    {
        await API.Call(() =>
        {
            _circuitCount++;
            WindowView windowView = new(API.MainContainer, $"Circuit {_circuitCount}")
            {
                WindowContent = new CircuitSheetView()
            };
            windowView.Maximize();
            API.TabBar?.AddWindow(windowView);
            return Task.CompletedTask;
        });

        FadeOut();
    }

    private bool OnCloseComponentsView()
    {
        _componentsWindowView?.Minimize();
        return true;
    }

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
            if (_preferencesWindowView == null)
            {
                _preferencesWindowView = new PreferencesWindowView(API.MainContainer);
                _preferencesWindowView.OnClose = () =>
                {
                    _preferencesWindowView = null;
                    return false;
                };

                API.TabBar?.AddWindow(_preferencesWindowView);
            }
            else
            {
                API.TabBar?.BringToFront(_preferencesWindowView);
            }

            return Task.CompletedTask;
        });

        FadeOut();
    }
}
