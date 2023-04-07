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
using SpiceSharp.Validation;

public class Simulation
{
    private readonly Dictionary<string, IExport<double>> _exports = new();
    private readonly Transient? _simulation;
    private EntityCollection? _circuit;
    private Worksheet? _worksheet;
    public Dictionary<IWorksheetItem, SimulationGraph>? Graphs { get; set; }
    public Action<string>? LogMethod { get; set; }

    public Simulation()
    {
        _simulation = new Transient("default", 0.1, 2);
        _simulation.ExportSimulationData += ExportSimulationData;
    }

    public void Prepare(Worksheet worksheet)
    {
        _circuit = new EntityCollection();
        _worksheet = worksheet;

        if (Graphs != null)
        {
            _exports.Clear();
            foreach (KeyValuePair<IWorksheetItem, SimulationGraph> graphItem in Graphs)
            {
                switch (graphItem.Key)
                {
                    case TraceItem netItem:
                        {
                            RealVoltageExport netVoltageExport = new(_simulation, netItem.Net.RefName);
                            _exports.Add($"{netItem.Net.RefName}V", netVoltageExport);
                            RealCurrentExport netCurrentExport = new(_simulation, netItem.Net.RefName);
                            _exports.Add($"{netItem.Net.RefName}A", netCurrentExport);
                            break;
                        }
                    case WorksheetItem item:
                        {
                            RealVoltageExport itemVoltageExport = new(_simulation, item.RefName);
                            _exports.Add($"{item.RefName}V", itemVoltageExport);
                            RealCurrentExport itemCurrentExport = new(_simulation, item.RefName);
                            _exports.Add($"{item.RefName}A", itemCurrentExport);
                            break;
                        }
                }
            }
        }

        foreach (IWorksheetItem item in worksheet.Items)
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
                    if (inductor.Value != null)
                    {
                        double inductance = inductor.Value.ParsePrefixesToDouble();
                        entity = new Inductor(item.RefName, GetNet(inductor, 0), GetNet(inductor, 1), inductance);
                    }

                    break;

                case DiodeItem diode:
                    entity = new Diode(item.RefName, GetNet(diode, 0), GetNet(diode, 1), "");
                    break;

                case ResistorItem resistor:
                    if (resistor.Value != null)
                    {
                        double prefixesToDouble = resistor.Value.ParsePrefixesToDouble();
                        entity = new Resistor(item.RefName, GetNet(resistor, 0), GetNet(resistor, 1), prefixesToDouble);
                    }

                    break;

                case PnpTransistorItem pnpTransistor:
                    BipolarJunctionTransistor pnp = new(item.RefName, GetNet(pnpTransistor, 0),
                        GetNet(pnpTransistor, 1), GetNet(pnpTransistor, 2), "", pnpTransistor.Name + "m");
                    BipolarJunctionTransistorModel pnpModel = new(pnpTransistor.Name + "m");
                    pnpModel.Parameters.SetPnp(true);
                    entity = pnp;
                    break;

                case TerminalItem:
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
            foreach (IRuleViolation? error in validationException.Rules.Violations)
            {
                if (error is VariablePresenceRuleViolation varError)
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
        foreach (KeyValuePair<string, IExport<double>> export in _exports)
        {
            LogMethod?.Invoke($"{export.Key} - {export.Value.Value}");
        }
    }

    private string GetNet(WorksheetItem item, int portNum)
    {
        PinDrawable currentPin = item.Pins[portNum];
        NetItem? netItem = _worksheet?.Nets.Cast<NetItem>().FirstOrDefault(net => net.Pins.Any(pin => pin.Equals(currentPin.ComponentGuid)));
        if (netItem == null)
        {
            return "";
        }

        List<IWorksheetItem>? itemsInNet = _worksheet?.Items.Where(
            i => i.Pins.Any(pin => netItem.Pins.Any(niPin => niPin.Equals(pin.ComponentGuid)))
        ).ToList();

        if (itemsInNet == null)
        {
            return "";
        }

        IWorksheetItem? gnd = itemsInNet.FirstOrDefault(i => i is TerminalItem);
        return gnd != null ? "0" : netItem.RefName;
    }
}
