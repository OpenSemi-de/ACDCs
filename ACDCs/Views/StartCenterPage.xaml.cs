using ACDCs.Services;
using ACDCs.Views.Components.Debug;
using ACDCs.Views.Components.Window;
using ContentPage = Microsoft.Maui.Controls.ContentPage;
using WindowView = ACDCs.Views.Components.Window.WindowView;

namespace ACDCs.Views;

public static class StartCenter
{
    public static Page Instance { get; set; }
}

public partial class StartCenterPage : ContentPage
{
    private int _circuitCount = 0;
    private ComponentsView _componentsView;
    private WindowView? _componentsWindowView;
    private DebugWindow _debugWindow;

    public StartCenterPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        StartCenter.Instance = this;
    }

    private async void Button_OnClicked(object? sender, EventArgs e)
    {
        await App.Call(() =>
         {
             _circuitCount++;
             WindowView windowView = new WindowView(MainWindowLayout, $"Circuit {_circuitCount}")
                 .WindowContent(new CircuitSheetView());
             windowView.Maximize();
             windowTabBar.AddWindow(windowView);
             return Task.CompletedTask;
         });
    }

    private async void ComponentsButton_OnClicked(object? sender, EventArgs e)
    {
        await App.Call(() =>
        {
            if (_componentsWindowView == null)
            {
                _componentsView = new ComponentsView();

                _componentsWindowView = new WindowView(MainWindowLayout, "Components")
                    .WindowContent(_componentsView);
                _componentsWindowView.OnClose = OnCloseComponentsView;

                windowTabBar.AddWindow(_componentsWindowView);
            }

            _componentsWindowView?.Maximize();
            return Task.CompletedTask;
        });
    }

    private bool OnCloseComponentsView()
    {
        _componentsWindowView?.Minimize();
        return true;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        BackgroundImageSource = ImageService.BackgroundImageSource(this);
        _debugWindow = new DebugWindow(MainWindowLayout) { StartCenterPage = this, TabBar = windowTabBar };
        windowTabBar.AddWindow(_debugWindow);
        _debugWindow.Minimize();
    }
}
