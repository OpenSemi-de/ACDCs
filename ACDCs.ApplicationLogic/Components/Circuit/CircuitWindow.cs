namespace ACDCs.ApplicationLogic.Components.Circuit;

using Sharp.UI;
using Window;

// ReSharper disable once UnusedType.Global
public class CircuitWindow : Window
{
    public CircuitWindow(WindowContainer? container) : base(container, "Circuit view", "menu_main.json", true, GetView)
    {
        Start();
    }

    private static View GetView(Window window)
    {
        if (window.ChildLayout is not WindowContainer container)
        {
            return new Label(" Error loading CircuitWindow");
        }

        CircuitSheetView circuitSheetView = new(container);
        window.MenuParameters.Add("CircuitView", circuitSheetView.CircuitView);
        window.Maximize();
        return circuitSheetView;
    }
}
