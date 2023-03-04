namespace ACDCs.API.Interfaces;

public interface IGetPropertyUpdates
{
    void OnPropertyUpdated(string? propertyName, object value);
}
