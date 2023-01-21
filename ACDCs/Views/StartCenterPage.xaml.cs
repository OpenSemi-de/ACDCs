using ACDCs.Services;
using ACDCs.Views.Debug;
using ContentPage = Microsoft.Maui.Controls.ContentPage;

namespace ACDCs.Views;

public partial class StartCenterPage : ContentPage
{
    private DebugWindow? _debugWindow;

    public StartCenterPage()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        API.MainPage = this;
        API.MainContainer = MainWindowLayout;
        PointerGestureRecognizer pointerMovement = new();
        pointerMovement.PointerMoved += PointerMovementOnPointerMoved;
        MainWindowLayout.GestureRecognizers.Add(pointerMovement);
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
