using ACDCs.ApplicationLogic.Components.Window;
using Microsoft.Maui.Layouts;
using WindowTabBar = ACDCs.ApplicationLogic.Components.Window.WindowTabBar;

#pragma warning disable IDE0065

namespace ACDCs.ApplicationLogic;

using Sharp.UI;

public class Workbench : ContentPage
{
    private readonly API _api;
    private readonly Grid _mainGrid;
    private readonly AbsoluteLayout? _mainWindowLayout;
    private readonly WindowStarterFrame _starterFrame;
    private readonly WindowTabBar _windowTabBar;

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

        _mainWindowLayout = new AbsoluteLayout();
        _mainGrid.Add(_mainWindowLayout);

        _starterFrame = new WindowStarterFrame();
        AbsoluteLayout.SetLayoutFlags(_starterFrame, AbsoluteLayoutFlags.YProportional);
        AbsoluteLayout.SetLayoutBounds(_starterFrame, new Rect(0, 1, 200, 400));
        _mainWindowLayout.Add(_starterFrame);

        _windowTabBar = new WindowTabBar(_starterFrame)
            .HorizontalOptions(LayoutOptions.Fill)
            .HeightRequest(42);

        Grid.SetRow(_windowTabBar, 1);
        _mainGrid.Add(_windowTabBar);

        Content = _mainGrid;

        Loaded += OnLoaded;
        API.MainPage = this;
        API.MainContainer = _mainWindowLayout;
        PointerGestureRecognizer pointerMovement = new();
        pointerMovement.PointerMoved += PointerMovementOnPointerMoved;
        _mainWindowLayout.GestureRecognizers.Add(pointerMovement.MauiObject);

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
