namespace ACDCs.API.Core.Components.Circuit;

using ACDCs.API.Windowing.Components.Window;
using Sharp.UI;

// ReSharper disable once UnusedType.Global
public class CircuitWindow : Window
{
    public CircuitSheetView? SheetView { get; set; }

    public CircuitWindow(WindowContainer? container) : base(container, "Circuit view", "menu_main.json", true, GetView)
    {
        Start();
    }

    private static View GetView(Window window)
    {
        if ((CircuitWindow)window is not { } circuitWindow)
        {
            return new Label(" Error loading CircuitWindow");
        }

        if (window.ChildLayout is not WindowContainer container)
        {
            return new Label(" Error loading CircuitWindow");
        }

        CircuitSheetView circuitSheetView = new(container);
        circuitWindow.SheetView = circuitSheetView;
        circuitWindow.MenuParameters.Add("CircuitView", circuitSheetView.CircuitView);
        circuitWindow.Maximize();
        return circuitSheetView;
    }
}
