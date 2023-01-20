using ACDCs.Services;
using ACDCs.Views.Debug;
using ContentPage = Microsoft.Maui.Controls.ContentPage;
using WindowView = ACDCs.Components.Window.WindowView;

namespace ACDCs.Views;

public partial class StartCenterPage : ContentPage
{
    private int _circuitCount;
    private ComponentsView? _componentsView;
    private WindowView? _componentsWindowView;
    private DebugWindow? _debugWindow;

    public StartCenterPage()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        API.MainPage = this;
        PointerGestureRecognizer pointerMovement = new();
        pointerMovement.PointerMoved += PointerMovementOnPointerMoved;
        MainWindowLayout.GestureRecognizers.Add(pointerMovement);
    }

    private async void ComponentsButton_OnClicked(object? sender, EventArgs e)
    {
        await API.Call(() =>
        {
            if (_componentsWindowView == null)
            {
                _componentsView = new ComponentsView();

                _componentsWindowView = new WindowView(MainWindowLayout, "Components")
                {
                    WindowContent = _componentsView,
                    OnClose = OnCloseComponentsView
                };

                windowTabBar.AddWindow(_componentsWindowView);
            }

            _componentsWindowView?.Maximize();
            return Task.CompletedTask;
        });
    }

    private async void NewCircuitButton_OnClicked(object? sender, EventArgs e)
    {
        await API.Call(() =>
         {
             _circuitCount++;
             WindowView windowView = new(MainWindowLayout, $"Circuit {_circuitCount}")
             {
                 WindowContent = new CircuitSheetView()
             };
             windowView.Maximize();
             windowTabBar.AddWindow(windowView);
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

    private void PointerMovementOnPointerMoved(object? sender, PointerEventArgs e)
    {
        Point? point = e.GetPosition(MainWindowLayout);
        if (API.PointerLayoutObjectToMeasure != null)
        {
            point = e.GetPosition(API.PointerLayoutObjectToMeasure);
        }

        if (point == null)
        {
            return;
        }

        API.PointerCallback?.Invoke((Point)point);
    }
}
