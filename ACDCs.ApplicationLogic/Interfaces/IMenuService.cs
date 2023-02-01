namespace ACDCs.ApplicationLogic.Interfaces;

public interface IMenuService
{
    void Add(string name, Action<object?> action);

    void Call(string menuCommand);

    void Call(string menuCommand, object param);
}
