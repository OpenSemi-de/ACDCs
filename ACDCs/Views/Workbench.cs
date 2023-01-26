using ACDCs.Components;
using ACDCs.Components.Window;
using ACDCs.Services;
using ACDCs.Views.Debug;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views;

using Sharp.UI;

public class Workbench : ContentPage
{
    private readonly Grid _mainGrid;
    private readonly SharpAbsoluteLayout _mainWindowLayout;
    private readonly WindowStarterFrame _starterFrame;
    private readonly WindowTabBar _windowTabBar;
    private DebugWindow? _debugWindow;

    public Workbench()
    {
        ColumnDefinitionCollection columns = new(new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star));
        RowDefinitionCollection rows = new(new[]
        {
            new Microsoft.Maui.Controls.RowDefinition(GridLength.Star),
            new Microsoft.Maui.Controls.RowDefinition(new GridLength(43))
        });

        _mainGrid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .ColumnDefinitions(columns)
            .RowDefinitions(rows);

        _mainWindowLayout = new SharpAbsoluteLayout();
        _mainGrid.Add(_mainWindowLayout);

        _starterFrame = new WindowStarterFrame();
        AbsoluteLayout.SetLayoutFlags(_starterFrame, AbsoluteLayoutFlags.YProportional);
        AbsoluteLayout.SetLayoutBounds(_starterFrame, new Rect(0, 1, 200, 400));
        _mainWindowLayout.Add(_starterFrame);

        _windowTabBar = new WindowTabBar()
            .HorizontalOptions(LayoutOptions.Fill)
            .HeightRequest(42)
            .StarterFrame(_starterFrame);
        Grid.SetRow(_windowTabBar, 1);
        _mainGrid.Add(_windowTabBar);

        Content = _mainGrid;

        Loaded += OnLoaded;
        API.MainPage = this;
        API.MainContainer = _mainWindowLayout;
        PointerGestureRecognizer pointerMovement = new();
        pointerMovement.PointerMoved += PointerMovementOnPointerMoved;
        _mainWindowLayout.GestureRecognizers.Add(pointerMovement.MauiObject);
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        BackgroundImageSource = ImageService.BackgroundImageSource(this);

        if (Convert.ToBoolean(API.GetPreference("StartDebugConsole")))
        {
            _debugWindow = new DebugWindow(_mainWindowLayout) { StartCenterPage = this, TabBar = _windowTabBar };
            _windowTabBar.AddWindow(_debugWindow);
            _debugWindow.Minimize();
        }
    }

    private void PointerMovementOnPointerMoved(object? sender, PointerEventArgs e)
    {
        Point? point = e.GetPosition(_mainWindowLayout);
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
