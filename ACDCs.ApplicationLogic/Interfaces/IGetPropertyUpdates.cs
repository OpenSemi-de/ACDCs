namespace ACDCs.ApplicationLogic.Components.Properties;

public interface IGetPropertyUpdates
{
    void OnPropertyUpdated(string? propertyName, object value);
}
