namespace ACDCs.API.Core;

using Instance;
using Microsoft.Maui.Layouts;
using Services;
using Windowing.Components.Window;

public class Workbench : ContentPage
{
    private readonly API _api;
    private Grid? _mainGrid;
    private WindowStarterFrame? _starterFrame;
    private WindowContainer? _windowContainer;
    private WindowTabBar? _windowTabBar;

    public Workbench(API api)
    {
        _api = api;
        Loaded += Load;
    }

    public static API GetAPIInstance()
    {
        return new API(
            new WorkbenchService(),
            new ImageService(),
            new ColorService(API.Resources, API.UserAppTheme),
            new DescriptionService(),
            new EditService(),
            new MenuService(),
            new FileService(),
            new ImportService()
        );
    }

    private void Load(object? o, EventArgs eventArgs)
    {
        ColumnDefinitionCollection columns = new(new ColumnDefinition(GridLength.Star));
        RowDefinitionCollection rows = new(new RowDefinition(GridLength.Star), new RowDefinition(new GridLength(43)));

        _mainGrid = new Grid()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .ColumnDefinitions(columns)
            .RowDefinitions(rows);

        _windowContainer = new WindowContainer();
        _mainGrid.Add(_windowContainer);

        _starterFrame = new WindowStarterFrame(_windowContainer);
        AbsoluteLayout.SetLayoutFlags(_starterFrame, AbsoluteLayoutFlags.YProportional);
        AbsoluteLayout.SetLayoutBounds(_starterFrame, new Rect(0, 1, 200, 400));
        _windowContainer.Add(_starterFrame);

        _windowTabBar = new WindowTabBar(_starterFrame)
            .HorizontalOptions(LayoutOptions.Fill)
            .HeightRequest(42);

        _windowContainer.TabBar = _windowTabBar;

        Grid.SetRow(_windowTabBar, 1);
        _mainGrid.Add(_windowTabBar);

        Content = _mainGrid;

        API.MainPage = this;
        API.MainContainer = _windowContainer;
        PointerGestureRecognizer pointerMovement = new();
        pointerMovement.PointerMoved += PointerMovementOnPointerMoved;
        _windowContainer.GestureRecognizers.Add(pointerMovement);

        // SizeChanged += OnSizeChanged;

        //     BackgroundImageSource = _api.BackgroundImageSource(this);
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        _windowTabBar?.OnSizeChanged();
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
