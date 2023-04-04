namespace ACDCs.API.Core.Components.Simulation;

using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Items.Sources;
using ACDCs.CircuitRenderer.Items.Transistors;
using ACDCs.Data.ACDCs.Components.Source;
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

public class Simulation
{
    private EntityCollection? _circuit;

    private SpiceSharp.Simulations.Transient? _simulation;

    private Worksheet? _worksheet;

    private RealVoltageExport? _zeroAnalysis;

    public Action<string>? LogMethod { get; set; }

    public Simulation()
    {
        _simulation = new SpiceSharp.Simulations.Transient("default", 0.1, 2);
        _simulation.ExportSimulationData += ExportSimulationData;
        _zeroAnalysis = new RealVoltageExport(_simulation, "0");
    }

    public void Prepare(Worksheet worksheet)
    {
        _circuit = new EntityCollection();
        _worksheet = worksheet;

        foreach (var item in worksheet.Items)
        {
            IEntity? entity = null;

            switch (item)
            {
                case CapacitorItem capacitor:
                    double capacity = capacitor.Value.ParsePrefixesToDouble();
                    entity = new Capacitor(item.RefName, GetNet(capacitor, 0), GetNet(capacitor, 1), capacity);
                    break;

                case InductorItem inductor:
                    double inductance = inductor.Value.ParsePrefixesToDouble();
                    entity = new Inductor(item.RefName, GetNet(inductor, 0), GetNet(inductor, 1), inductance);
                    break;

                case DiodeItem diode:
                    entity = new Diode(item.RefName, GetNet(diode, 0), GetNet(diode, 1), "");
                    break;

                case ResistorItem resistor:
                    double prefixesToDouble = resistor.Value.ParsePrefixesToDouble();
                    entity = new Resistor(item.RefName, GetNet(resistor, 0), GetNet(resistor, 1), prefixesToDouble);
                    break;

                case PnpTransistorItem pnpTransistor:
                    BipolarJunctionTransistor pnp = new(item.RefName, GetNet(pnpTransistor, 0),
                        GetNet(pnpTransistor, 1), GetNet(pnpTransistor, 2), "", pnpTransistor.Name + "m");
                    BipolarJunctionTransistorModel pnpModel = new(pnpTransistor.Name + "m");
                    pnpModel.Parameters.SetPnp(true);
                    entity = pnp;
                    break;

                case TerminalItem terminal:
                    break;

                case VoltageSourceItem voltageSource:
                    if (voltageSource.Model?.Type == "DC")
                    {
                        SourceParameters? dcModel = voltageSource.Model as SourceParameters;
                        if (dcModel?.DcValue != null)
                        {
                            entity = new VoltageSource(item.RefName, GetNet(voltageSource, 0),
                                GetNet(voltageSource, 1), dcModel.DcValue);
                        }
                    }

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
            if (_simulation != null)
            {
                _simulation.Run(_circuit);
            }
        }
        catch (ValidationFailedException validationException)
        {
            foreach (var error in validationException.Rules.Violations)
            {
                if (error is SpiceSharp.Validation.VariablePresenceRuleViolation varError)
                    LogMethod?.Invoke($"{varError.Variable}");
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void Stop()
    {
    }

    private void ExportSimulationData(object? sender, ExportDataEventArgs e)
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
        return gnd != null ? "0" : netItem.RefName;
    }
}
