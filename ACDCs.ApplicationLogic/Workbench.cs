namespace ACDCs.ApplicationLogic;

using ACDCs.ApplicationLogic.Components.Window;
using Microsoft.Maui.Layouts;
using Sharp.UI;

public class Workbench : ContentPage
{
    private readonly API _api;
    private readonly Grid _mainGrid;
    private readonly WindowStarterFrame _starterFrame;
    private readonly WindowContainer? _windowContainer;
    private readonly WindowTabBar _windowTabBar;

    public WindowContainer? WindowContainer => _windowContainer;

    public Workbench(API api)
    {
        _api = api;
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

        _windowContainer = new();
        _mainGrid.Add(_windowContainer);

        _starterFrame = new WindowStarterFrame(_windowContainer);
        AbsoluteLayout.SetLayoutFlags(_starterFrame, AbsoluteLayoutFlags.YProportional);
        AbsoluteLayout.SetLayoutBounds(_starterFrame, new Rect(0, 1, 200, 400));
        _windowContainer.Add(_starterFrame);

        _windowTabBar = new WindowTabBar(_starterFrame)
            .HorizontalOptions(LayoutOptions.Fill)
            .HeightRequest(42);

        Grid.SetRow(_windowTabBar, 1);
        _mainGrid.Add(_windowTabBar);

        Content = _mainGrid;

        Loaded += OnLoaded;
        API.MainPage = this;
        API.MainContainer = _windowContainer;
        PointerGestureRecognizer pointerMovement = new();
        pointerMovement.PointerMoved += PointerMovementOnPointerMoved;
        _windowContainer.GestureRecognizers.Add(pointerMovement.MauiObject);

        SizeChanged += OnSizeChanged;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        BackgroundImageSource = _api.BackgroundImageSource(this);
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        _windowTabBar.OnSizeChanged();
    }

    private void PointerMovementOnPointerMoved(object? sender, PointerEventArgs e)
    {
        Point? point = e.GetPosition(_windowContainer);
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
