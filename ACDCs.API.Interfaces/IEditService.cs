namespace ACDCs.API.Interfaces;

public interface IEditService
{
    Task Delete(ICircuitView circuitView);

    Task DeselectAll(ICircuitView circuitView);

    Task Duplicate(ICircuitView circuitView);

    Task Mirror(ICircuitView circuitView);

    Task Rotate(ICircuitView circuitView);

    Task SelectAll(ICircuitView circuitView);

    Task ShowProperties(ICircuitView circuitView);

    Task SwitchMultiselect(object? state, ICircuitView circuitView);
}
