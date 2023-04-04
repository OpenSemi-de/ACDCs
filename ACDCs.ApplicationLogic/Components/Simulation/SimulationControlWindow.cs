namespace ACDCs.API.Core.Components.Simulation;

using ACDCs.API.Windowing.Components.Window;
using CircuitRenderer.Interfaces;

public class SimulationControlWindow : Window
{
    private static SimulationControlView? _simulationControlView;

    public SimulationLogWindow? LogWindow { get; set; }

    public SimulationControlWindow(WindowContainer? container) : base(container, "Simulation", "", false, GetView, 20)
    {
        Start();
        HideResizer();
        HideWindowButtons();
        container?.SetWindowPosition(this, 5, 405);
        container?.SetWindowSize(this, 104, 330);
    }

    public void SelectItem(IWorksheetItem item)
    {
        _simulationControlView?.SelectItem(item);
    }

    public void SetSimulation(SimulationController simulation)
    {
        _simulationControlView?.SetSimulation(simulation);
    }

    private static View GetView(Window arg)
    {
        _simulationControlView = new SimulationControlView();
        return _simulationControlView;
    }
}
