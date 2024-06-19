namespace ACDCs.API.Windowing.Components.Window;

using Instance;
using Interfaces;
using Sharp.UI;
using UraniumUI.Icons.FontAwesome;

public class WindowTabBar : Grid, IWindowTabBarProperties, IWindowTabBar
{
    private readonly StackLayout _mainLayout;
    private readonly ScrollView _scrollView;

    private readonly Button _starterButton;
    private readonly Dictionary<WindowTab, Window> _windows;
    private Window? _focusWindow;

    public IWindowStarterFrame? StarterFrame { get; set; }

    public WindowTabBar(WindowStarterFrame? starterFrame)
    {
        StarterFrame = starterFrame;
        API.TabBar = this;
        ColumnDefinitions.Add(new ColumnDefinition(84));
        ColumnDefinitions.Add(new ColumnDefinition());
        RowDefinitions.Add(new RowDefinition(43));

        _starterButton = new Button(Solid.Atom)
            .FontFamily("FASolid")
            .FontSize(20)
            .FontAttributes(FontAttributes.Bold)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .OnClicked(OnStarterButton_Clicked);

        _starterButton.BackgroundColor = API.Instance.Foreground;
        _starterButton.TextColor = API.Instance.Text;
        _starterButton.BorderColor = API.Instance.Border;

        _windows = new Dictionary<WindowTab, Window>();

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

    public void AddWindow(Window window)
    {
        WindowTab tab = new(window.WindowTitle, OnTabClicked);
        //  window.TabBar = this;
        TapGestureRecognizer tapGestureRecognizer = new();
        tapGestureRecognizer.Tapped += TapGestureRecognizerOnTapped;
        window.GestureRecognizers(
            tapGestureRecognizer
        );
        window.OnFocused(OnWindowFocus);

        BringToFront(window);
        _windows.Add(tab, window);
        _mainLayout.Add(tab);
        MarkFocused();
    }

    public void BringToFront(object? sender)
    {
        if (sender == null)
        {
            return;
        }

        Window window = (Window)sender;
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

    public void BringToFront(IWindow? window)
    {
        BringToFront(window as object);
    }

    public void OnSizeChanged()
    {
        foreach (Window window in _windows.Values)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                window.Maximize();
            }
        }
    }

    public async void RemoveWindow(Window window)
    {
        await API.Call(() =>
         {
             if (!_windows.ContainsValue(window))
             {
                 return Task.CompletedTask;
             }

             WindowTab tab = _windows.First(kv => kv.Value == window).Key;
             _windows.Remove(tab);
             _mainLayout.Remove(tab);
             return Task.CompletedTask;
         });
    }

    private void Debug()
    {
    }

    private async void MarkFocused()
    {
        await API.Call(() =>
        {
            foreach (WindowTab windowTab in _windows.Keys)
            {
                windowTab.SetInactive();
                _windows[windowTab].SetInactive();
            }

            if (_focusWindow == null)
            {
                return Task.CompletedTask;
            }

            if (!_windows.ContainsValue(_focusWindow))
            {
                return Task.CompletedTask;
            }

            KeyValuePair<WindowTab, Window>? focusTab = _windows.First(kv => kv.Value == _focusWindow);
            focusTab?.Key.SetActive();

            _focusWindow.SetActive();
            return Task.CompletedTask;
        });
    }

    private void OnStarterButton_Clicked(object? sender, EventArgs e)
    {
        StarterFrame?.Load();
        StarterFrame?.FadeIn();
    }

    private async void OnTabClicked(WindowTab tab)
    {
        await API.Call(() =>
           {
               if (!_windows.ContainsKey(tab))
               {
                   return Task.CompletedTask;
               }

               Window window = _windows[tab];
               if (window.WindowState == WindowState.Minimized)
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
