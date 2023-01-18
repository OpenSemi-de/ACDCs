using ACDCs.Views.Window;

namespace ACDCs.Components.Window;

using Sharp.UI;

public class WindowTabBar : Grid
{
    private readonly StackLayout _mainLayout;
    private readonly ScrollView _scrollView;

    private readonly Dictionary<WindowTab, WindowView> _windowViews;
    private WindowView? _focusWindow;

    public WindowTabBar()
    {
        ColumnDefinitions.Add(
            new(GridLength.Star)
        );
        RowDefinitions.Add(
            new(new GridLength(34))
        );
        _windowViews = new();

        _mainLayout = new StackLayout()
            .VerticalOptions(LayoutOptions.Fill)
            .Orientation(StackOrientation.Horizontal);

        _scrollView = new ScrollView()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .HorizontalScrollBarVisibility(ScrollBarVisibility.Always)
            .HeightRequest(34);
        _scrollView.Content = _mainLayout;

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
        if (sender != null)
        {
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
    }

    public void RemoveWindow(WindowView windowView)
    {
        if (_windowViews.ContainsValue(windowView))
        {
            WindowTab tab = _windowViews.First(kv => kv.Value == windowView).Key;
            _windowViews.Remove(tab);
            _mainLayout.Remove(tab);
            windowView.TabBar = null;
        }
    }

    private void MarkFocused()
    {
        foreach (WindowTab windowTab in _windowViews.Keys)
        {
            windowTab.SetInactive();
            _windowViews[windowTab].SetInactive();
        }

        if (_focusWindow != null)
        {
            KeyValuePair<WindowTab, WindowView>? focusTab = _windowViews.First(kv => kv.Value == _focusWindow);
            focusTab?.Key.SetActive();

            _focusWindow.SetActive();
        }
    }

    private async void OnTabClicked(WindowTab tab)
    {
        await API.Call(() =>
           {
               if (_windowViews.ContainsKey(tab))
               {
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
               }
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
