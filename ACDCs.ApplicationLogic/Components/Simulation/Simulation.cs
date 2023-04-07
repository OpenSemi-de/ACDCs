namespace ACDCs.API.Core.Components.Simulation;

using CircuitRenderer.Drawables;
using CircuitRenderer.Interfaces;
using CircuitRenderer.Items;
using CircuitRenderer.Items.Sources;
using CircuitRenderer.Items.Transistors;
using CircuitRenderer.Sheet;
using Data.ACDCs.Components.Source;
using SpiceSharp.Components;
using SpiceSharp.Entities;
using SpiceSharp.Simulations;

public class Simulation
{
    private readonly Transient? _simulation;
    private readonly RealVoltageExport? _zeroAnalysis;
    private EntityCollection? _circuit;
    private Dictionary<string, IExport<double>> _exports = new();
    private Worksheet? _worksheet;
    public Dictionary<IWorksheetItem, SimulationGraph>? Graphs { get; set; }
    public Action<string>? LogMethod { get; set; }

    public Simulation()
    {
        _simulation = new Transient("default", 0.1, 2);
        _simulation.ExportSimulationData += ExportSimulationData;
        _zeroAnalysis = new RealVoltageExport(_simulation, "0");
        _exports.Add("0", _zeroAnalysis);
    }

    public string GetNet(WorksheetItem item, int portNum)
    {
        PinDrawable currentPin = item.Pins[portNum];
        NetItem? netItem = _worksheet?.Nets.Cast<NetItem>().FirstOrDefault(net => net.Pins.Any(pin => pin.Equals(currentPin.ComponentGuid))) as NetItem;
        if (netItem == null)
        {
            return "";
        }

        var itemsInNet = _worksheet?.Items.Where(
            i => i.Pins.Any(pin => netItem.Pins.Any(niPin => niPin.Equals(pin.ComponentGuid)))
        ).ToList();

        if (itemsInNet == null)
        {
            return "";
        }

        var gnd = itemsInNet.FirstOrDefault(i => i is TerminalItem);
        return gnd != null ? "0" : netItem.RefName;
    }

    public void Prepare(Worksheet worksheet)
    {
        _circuit = new EntityCollection();
        _worksheet = worksheet;

        foreach (KeyValuePair<IWorksheetItem, SimulationGraph> graphItem in Graphs)
        {
            if (graphItem.Key is TraceItem netItem)
            {
                RealVoltageExport netExport = new RealVoltageExport(_simulation, netItem.Net.RefName);
                _exports.Add(netItem.Net.RefName, netExport);
            }
        }

        foreach (var item in worksheet.Items)
        {
            IEntity? entity = null;

            switch (item)
            {
                case CapacitorItem capacitor:
                    if (capacitor.Value != null)
                    {
                        double capacity = capacitor.Value.ParsePrefixesToDouble();
                        entity = new Capacitor(item.RefName, GetNet(capacitor, 0), GetNet(capacitor, 1), capacity);
                    }

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
            _simulation?.Run(_circuit);
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
            LogMethod?.Invoke(ex.ToString());
        }
    }

    public void Stop()
    {
    }

    private void ExportSimulationData(object? sender, ExportDataEventArgs e)
    {
        LogMethod?.Invoke($"{e.Time}");
        foreach (var export in _exports)
        {
            LogMethod?.Invoke($"{export.Key} - {export.Value.Value}");
        }
    }
}
