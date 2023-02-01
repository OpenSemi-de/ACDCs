using ACDCs.ApplicationLogic.Components.Circuit;

namespace ACDCs.ApplicationLogic.Interfaces;

public interface IFileService
{
    Task NewFile(CircuitView circuitView);

    Task OpenFile(CircuitView circuitView);

    Task SaveFile(CircuitView circuitView, Page popupPage);

    Task SaveFileAs(Page popupPage, CircuitView circuitView);
}
