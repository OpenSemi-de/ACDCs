using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Items.Transistors;
using ACDCs.Services;
using ACDCs.Views.Components.WindowView;

namespace ACDCs.Views;

public partial class StartCenterPage : ContentPage
{
    public StartCenterPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void Button_OnClicked(object? sender, EventArgs e)
    {
        WindowView windowView = new WindowView(MainWindowLayout)
            .WindowContent(new CircuitSheetView());
        windowView.Maximize();
        MainWindowLayout.Add(windowView);
    }

    private async void CircuitView_OnLoaded(object? sender, EventArgs e)
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
    }

    private void ComponentsButton_OnClicked(object? sender, EventArgs e)
    {
        ComponentsView componentsView = new ComponentsView();

        WindowView windowView = new WindowView(MainWindowLayout)
            .WindowContent(componentsView);
        windowView.OnClose = componentsView.OnClose;
        MainWindowLayout.Add(windowView);

        windowView.Maximize();
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        BackgroundImageSource = ImageService.BackgroundImageSource(this);
    }
}
