namespace ACDCs.API.Interfaces;

public interface IDescriptionService
{
    string GetComponentDescription(Type parentType, string propertyName);

    int GetComponentPropertyOrder(Type parentType, string propertyName);
}
