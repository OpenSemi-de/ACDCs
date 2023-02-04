using ACDCs.ApplicationLogic.Components.Window;

namespace ACDCs.ApplicationLogic.Components.Circuit;

public class CircuitWindow : Window.Window
{
    public CircuitWindow(WindowContainer container) : base(container, "Circuit view", "menu_main.json", true, GetView)
    {
        Loaded += CircuitWindow_Loaded;
        Start();
    }

    private static View GetView(Window.Window window)
    {
        CircuitSheetView circuitSheetView = new();
        window.MenuParameters.Add("CircuitView", circuitSheetView.CircuitView);
        return circuitSheetView;
    }

    private void CircuitWindow_Loaded(object? sender, EventArgs e)
    {
    }
}
