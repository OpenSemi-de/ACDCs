namespace ACDCs.API.Core.Components.Simulation;

using Windowing.Components.Window;

public class SimulationLogWindow : Window
{
    private SimulationLogView? _logView;

    public SimulationLogView? LogView
    {
        get => _logView;
        set => _logView = value;
    }

    public SimulationLogWindow(WindowContainer? container) : base(container, "Log", "", false, GetView, 20)
    {
        Start();
    }

    public void AddLog(string text)
    {
        LogView.AddLog(text);
    }

    public void VisibilityChanged(bool visible)
    {
        this.IsVisible = visible;
    }

    private static View GetView(Window arg)
    {
        if (arg is not SimulationLogWindow window) return new Label("Error");
        window.LogView = new SimulationLogView();
        return window.LogView;
    }
}
