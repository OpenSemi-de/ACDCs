using ACDCs.Services;
using ACDCs.Views;
using Sharp.UI;
using UraniumUI.Icons.FontAwesome;
using Button = Sharp.UI.Button;
using ColumnDefinition = Microsoft.Maui.Controls.ColumnDefinition;
using Grid = Sharp.UI.Grid;
using RowDefinition = Microsoft.Maui.Controls.RowDefinition;
using ScrollView = Sharp.UI.ScrollView;
using StackLayout = Sharp.UI.StackLayout;
using TapGestureRecognizer = Sharp.UI.TapGestureRecognizer;

namespace ACDCs.Components.Window;

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
        ColumnDefinitions.Add(new ColumnDefinition(new GridLength(84)));
        ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        RowDefinitions.Add(new RowDefinition(new GridLength(43)));

        _starterButton = new Button(Solid.Atom)
            .FontFamily("FASolid")
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
        WindowTab tab = new(window.WindowTitle, OnTabClicked);
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

        WindowView window = (WindowView)sender;
        if (_focusWindow != window)
        {
            if (_focusWindow != null)
            {
                window.ZIndex = _focusWindow.ZIndex + 1;
            }
            else
            {
                window.ZIndex++;
            }
        }

        _focusWindow = window;
    }

    public void OnSizeChanged()
    {
        foreach (WindowView windowView in _windowViews.Values)
        {
            if (windowView.State == WindowState.Maximized)
            {
                windowView.Maximize();
            }
        }
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

    private void OnStarterButton_Clicked(object? sender, EventArgs e)
    {
        StarterFrame.FadeIn();
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
