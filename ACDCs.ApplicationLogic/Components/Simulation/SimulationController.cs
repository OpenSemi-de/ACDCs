namespace ACDCs.API.Core.Components.Simulation;

using ACDCs.CircuitRenderer.Items;
using CircuitRenderer.Drawables;
using CircuitRenderer.Interfaces;
using CircuitRenderer.Sheet;
using SpiceSharp.Components;
using SpiceSharp.Entities;
using SpiceSharp.Simulations;

public class SimulationController
{
    private readonly Dictionary<IWorksheetItem, SimulationGraph> _graphs = new();
    private Simulation _simulation = new();
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
        PrepareSheet();
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

public class Simulation
{
    private EntityCollection? _circuit;
    private SpiceSharp.Simulations.Transient? _simulation;
    private Worksheet? _worksheet;

    public void Prepare(Worksheet worksheet)
    {
        _simulation = new SpiceSharp.Simulations.Transient("default");
        _circuit = new EntityCollection();
        _worksheet = worksheet;

        foreach (var item in worksheet.Items)
        {
            IEntity? entity = null;

            switch (item)
            {
                case ResistorItem resistor:
                    double prefixesToDouble = (double)resistor.Model?.Value.ParsePrefixesToDouble();
                    entity = new Resistor(item.RefName, GetNet(resistor, 0), GetNet(resistor, 1), prefixesToDouble);
                    break;

                case TerminalItem terminal:
                    break;
            }

            if (entity != null)
            {
                _circuit.Add(entity);
            }
        }
    }

    public void Run()
    {
        try
        {
            _simulation?.Run(_circuit);
        }
        catch (ValidationFailedException validationException)
        {
        }
        catch (Exception ex)
        {
        }
    }

    public void Stop()
    {
    }

    private string GetNet(WorksheetItem item, int portNum)
    {
        PinDrawable currentPin = item.Pins[portNum];
        NetItem? netItem = _worksheet?.Nets.FirstOrDefault(net => net.Pins.Contains(currentPin)) as NetItem;
        if (netItem == null)
        {
            return "";
        }

        var gnd = netItem.Pins.FirstOrDefault(p => p.ParentItem is TerminalItem);
        if (gnd != null)
        {
            return "0";
        }

        return netItem.RefName;
    }
}
