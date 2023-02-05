namespace ACDCs.ApplicationLogic.Interfaces;

using Components.Circuit;

public interface IFileService
{
    Task NewFile(CircuitView circuitView);

    Task OpenFile(CircuitView circuitView);

    Task SaveFile(CircuitView circuitView, Page popupPage);

    Task SaveFileAs(Page popupPage, CircuitView circuitView);
}
