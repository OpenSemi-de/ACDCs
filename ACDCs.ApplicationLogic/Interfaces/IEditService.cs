using ACDCs.ApplicationLogic.Components.Circuit;

namespace ACDCs.ApplicationLogic.Interfaces;

public interface IEditService
{
    Task Delete(CircuitView circuitView);

    Task DeselectAll(CircuitView circuitView);

    Task Duplicate(CircuitView circuitView);

    Task Mirror(CircuitView circuitView);

    Task Rotate(CircuitView circuitView);

    Task SelectAll(CircuitView circuitView);

    Task SwitchMultiselect(object? state, CircuitView circuitView);
}
