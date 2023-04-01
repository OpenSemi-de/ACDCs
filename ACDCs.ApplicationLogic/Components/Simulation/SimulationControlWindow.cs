namespace ACDCs.API.Core.Components.Simulation;

using ACDCs.API.Windowing.Components.Window;

public class SimulationControlWindow : Window
{
    public SimulationControlWindow(WindowContainer? container) : base(container, "Simulation", "", false, GetView, 20)
    {
        Start();
        HideResizer();
        HideWindowButtons();
        container?.SetWindowPosition(this, 5, 405);
        container?.SetWindowSize(this, 104, 300);
    }

    private static View GetView(Window arg)
    {
        return new SimulationControlView();
    }
}
