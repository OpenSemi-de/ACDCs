namespace ACDCs.API.Interfaces;

public interface IFileService
{
    Task NewFile(ICircuitView circuitView);

    Task OpenFile(ICircuitView circuitView);

    Task SaveFile(ICircuitView circuitView, Page popupPage);

    Task SaveFileAs(Page popupPage, ICircuitView circuitView);
}
