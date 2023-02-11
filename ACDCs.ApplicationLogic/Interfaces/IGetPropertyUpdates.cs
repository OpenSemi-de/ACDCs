namespace ACDCs.ApplicationLogic.Interfaces;

public interface IGetPropertyUpdates
{
    void OnPropertyUpdated(string? propertyName, object value);
}
