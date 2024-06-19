namespace ACDCs.API.Core.Components.Simulation;

using Windowing.Components.Window;

public class SimulationGraphWindow : Window
{
    private SimulationGraphView? _graphView;

    public SimulationGraphView? GraphView
    {
        get => _graphView;
        set => _graphView = value;
    }

    public SimulationGraphWindow(WindowContainer? container) : base(container, "Graphs", "", false, GetView, 20)
    {
        Start();
    }

    public void VisibilityChanged(bool visible)
    {
        this.IsVisible = visible;
    }

    private static View GetView(Window arg)
    {
        if (arg is not SimulationGraphWindow window) return new Label("Error");
        window.GraphView = new SimulationGraphView();
        return window.GraphView;
    }
}
