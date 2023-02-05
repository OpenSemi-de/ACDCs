using ACDCs.ApplicationLogic.Components.Window;

namespace ACDCs.ApplicationLogic.Components.Circuit;

using Sharp.UI;

public class CircuitWindow : Window.Window
{
    public CircuitWindow(WindowContainer? container) : base(container, "Circuit view", "menu_main.json", true, GetView)
    {
        Loaded += CircuitWindow_Loaded;
        Start();
    }

    private static View GetView(Window.Window window)
    {
        if (window.ChildLayout is WindowContainer container)
        {
            CircuitSheetView circuitSheetView = new(container);
            window.MenuParameters.Add("CircuitView", circuitSheetView.CircuitView);
            return circuitSheetView;
        }

        return new Label(" Error loading CircuitWindow");
    }

    private void CircuitWindow_Loaded(object? sender, EventArgs e)
    {
    }
}
