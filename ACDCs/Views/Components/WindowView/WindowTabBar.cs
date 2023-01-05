namespace ACDCs.Views.Components.WindowView;

using Sharp.UI;

public class WindowTabBar : Grid
{
    private readonly StackLayout _mainLayout;
    private readonly ScrollView _scrollView;

    private Dictionary<WindowTab, WindowView> _windowViews;

    public WindowTabBar()
    {
        this.ColumnDefinitions.Add(
            new(GridLength.Star)
        );
        this.RowDefinitions.Add(
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

        this.Add(_scrollView);
    }

    public void AddWindow(WindowView window)
    {
        var tab = new WindowTab(window.WindowTitle, OnTabClicked);
        window.TabBar = this;
        _windowViews.Add(tab, window);
        _mainLayout.Add(tab);
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

    private void OnTabClicked(WindowTab tab)
    {
        if (_windowViews.ContainsKey(tab))
        {
            WindowView window = _windowViews[tab];
            if (window.State == WindowState.Minimized)
            {
                window.Restore();
            }
            else
            {
                window.Minimize();
            }
        }
    }
}
