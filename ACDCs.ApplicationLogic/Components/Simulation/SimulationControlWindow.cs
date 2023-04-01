namespace ACDCs.API.Core.Components.Simulation;

using ACDCs.API.Windowing.Components.Window;

public class SimulationControlWindow : Window
{
    public SimulationControlWindow(WindowContainer? container) : base(container, "Simulation", "", false, GetView)
    {
        Start();
        HideResizer();
        HideWindowButtons();
        container?.SetWindowPosition(this, 5, 405);
        container?.SetWindowSize(this, 100, 300);
    }

    private static View GetView(Window arg)
    {
        return new SimulationControlView();
    }
}
