namespace ACDCs.API.Core.Components.Simulation;

using CircuitRenderer.Interfaces;
using CircuitRenderer.Sheet;

public class SimulationController
{
    private readonly Dictionary<IWorksheetItem, SimulationGraph> _graphs = new();
    private Simulation _simulation = new();
    public Func<Worksheet>? GetSheet { get; set; }
    public Action<string>? LogMethod { get; set; }
    public Worksheet? Sheet { get; set; }

    public void AddGraph(IWorksheetItem item)
    {
        if (!HasGraph(item))
            _graphs.Add(item, new SimulationGraph());
    }

    public bool HasGraph(IWorksheetItem item)
    {
        return _graphs.ContainsKey(item);
    }

    public void RemoveGraph(IWorksheetItem item)
    {
        if (HasGraph(item))
        {
            _graphs.Remove(item);
        }
    }

    public void Rewind()
    {
    }

    public void Start()
    {
        _simulation.Graphs = _graphs;
        Sheet = GetSheet?.Invoke();
        PrepareSheet();
        _simulation.LogMethod = LogMethod;
        _simulation.Run();
    }

    public void Stop()
    {
        _simulation.Stop();
    }

    private void PrepareSheet()
    {
        if (Sheet == null)
            return;

        _simulation.Prepare(Sheet);
    }
}
