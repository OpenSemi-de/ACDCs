using ACDCs.Services;
using ACDCs.Views;

namespace ACDCs.Components.Window;

using Sharp.UI;

public class WindowStarterFrame : Frame
{
    private readonly StackLayout _buttonStack;
    private readonly Grid _grid;
    private readonly ScrollView _scrollView;
    private int _circuitCount;
    private ComponentsView? _componentsView;
    private WindowView? _componentsWindowView;

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

        BorderColor = ColorService.Border;
        BackgroundColor = ColorService.BackgroundHigh;
        WidthRequest = 240;
        Padding = 0;
        Margin = 0;
        Content = _grid;
        this.ZIndex(1000);
        Opacity = 0;
    }

    private void FadeOut()
    {
        this.FadeTo(0, 500);
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
}

[SharpObject]
public partial class WindowTabBar : Grid, IWindowTabBarProperties
{
    private readonly StackLayout _mainLayout;
    private readonly ScrollView _scrollView;

    private readonly Button _starterButton;
    private readonly Dictionary<WindowTab, WindowView> _windowViews;
    private int _circuitCount;
    private ComponentsView _componentsView;
    private WindowView? _componentsWindowView;
    private WindowView? _focusWindow;

    public WindowTabBar()
    {
        API.TabBar = this;
        ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(new GridLength(84)));
        ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star));
        RowDefinitions.Add(new Microsoft.Maui.Controls.RowDefinition(new GridLength(43)));

        _starterButton = new Button("∆")
            .FontSize(20)
            .FontAttributes(FontAttributes.Bold)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .OnClicked(OnStarterButton_Clicked);

        _starterButton.BackgroundColor = ColorService.Foreground;
        _starterButton.TextColor = ColorService.Text;
        _starterButton.BorderColor = ColorService.Border;

        _windowViews = new Dictionary<WindowTab, WindowView>();

        _mainLayout = new StackLayout()
            .VerticalOptions(LayoutOptions.Fill)
            .Orientation(StackOrientation.Horizontal);

        _scrollView = new ScrollView()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .HorizontalScrollBarVisibility(ScrollBarVisibility.Always)
            .HeightRequest(42);
        _scrollView.Content = _mainLayout;

        Grid.SetColumn(_scrollView, 1);

        Add(_starterButton);
        Add(_scrollView);
    }

    public void AddWindow(WindowView window)
    {
        var tab = new WindowTab(window.WindowTitle, OnTabClicked);
        window.TabBar = this;
        TapGestureRecognizer tapGestureRecognizer = new();
        tapGestureRecognizer.Tapped += TapGestureRecognizerOnTapped;
        window.GestureRecognizers.Add(
            tapGestureRecognizer.MauiObject
        );
        window.OnFocused(OnWindowFocus);

        BringToFront(window);
        _windowViews.Add(tab, window);
        _mainLayout.Add(tab);
        MarkFocused();
    }

    public void BringToFront(object? sender)
    {
        if (sender == null)
        {
            return;
        }

        var window = (WindowView)sender;
        if (_focusWindow != window)
        {
            if (_focusWindow != null)
            {
                window.ZIndex = _focusWindow.ZIndex + 1;
            }
        }

        _focusWindow = window;
    }

    public void RemoveWindow(WindowView windowView)
    {
        if (!_windowViews.ContainsValue(windowView))
        {
            return;
        }

        WindowTab tab = _windowViews.First(kv => kv.Value == windowView).Key;
        _windowViews.Remove(tab);
        _mainLayout.Remove(tab);
        windowView.TabBar = null;
    }

    private void Debug()
    {
    }

    private void MarkFocused()
    {
        foreach (WindowTab windowTab in _windowViews.Keys)
        {
            windowTab.SetInactive();
            _windowViews[windowTab].SetInactive();
        }

        if (_focusWindow == null)
        {
            return;
        }

        KeyValuePair<WindowTab, WindowView>? focusTab = _windowViews.First(kv => kv.Value == _focusWindow);
        focusTab?.Key.SetActive();

        _focusWindow.SetActive();
    }

    private bool OnCloseComponentsView()
    {
        _componentsWindowView?.Minimize();
        return true;
    }

    private void OnStarterButton_Clicked(object? sender, EventArgs e)
    {
        StarterFrame.FadeTo(StarterFrame.Opacity == 1 ? 0 : 1, 500);
    }

    private async void OnTabClicked(WindowTab tab)
    {
        await API.Call(() =>
           {
               if (!_windowViews.ContainsKey(tab))
               {
                   return Task.CompletedTask;
               }

               WindowView window = _windowViews[tab];
               if (window.State == WindowState.Minimized)
               {
                   window.Restore();
                   window.Focus();
                   BringToFront(window);
                   MarkFocused();
                   return Task.CompletedTask;
               }

               if (_focusWindow == window)
               {
                   window.Minimize();
                   return Task.CompletedTask;
               }

               if (_focusWindow != window)
               {
                   BringToFront(window);
               }

               MarkFocused();
               return Task.CompletedTask;
           });
    }

    private void OnWindowFocus(object? sender, FocusEventArgs e)
    {
        BringToFront(sender);
        MarkFocused();
    }

    private void TapGestureRecognizerOnTapped(object? sender, TappedEventArgs e)
    {
        BringToFront(sender);
        MarkFocused();
    }
}

[BindableProperties]
public interface IWindowTabBarProperties
{
    WindowStarterFrame StarterFrame { get; set; }
}
