using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Items.Transistors;
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

    private async void CircuitView_OnLoaded(object? sender, EventArgs e)
    {
        await App.Call(async () =>
        {
            TextItem textItemLogo = new("ACDCs", 140, 10, 10);
            TextItem textItemText = new("Advanced Circuit Design Component Suite", 20, 10, 12);
            textItemLogo.IsRealFontSize = true;
            textItemText.IsRealFontSize = true;
            textItemLogo.Width = 10;
            textItemLogo.Height = 6;
            PnpTransistorItem pnp1 = new PnpTransistorItem();
            PnpTransistorItem pnp2 = new PnpTransistorItem();

            CircuitView.InsertToPosition(10, 4, pnp1).Wait();
            CircuitView.InsertToPosition(16, 4, pnp2).Wait();
            CircuitView.CurrentWorksheet.Nets.AddNet(pnp1.Pins[2], pnp2.Pins[2]);
            CircuitView.InsertToPosition(10, 10, textItemLogo).Wait();
            CircuitView.InsertToPosition(10, 12, textItemText).Wait();
            await CircuitView.Paint();
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
