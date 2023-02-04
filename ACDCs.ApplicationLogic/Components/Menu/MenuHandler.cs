namespace ACDCs.ApplicationLogic.Components.Menu;

public class MenuHandler
{
    private Dictionary<string, object> Parameters { get; set; } = new();

    public T? GetParameter<T>(string name) where T : class
    {
        return !Parameters.ContainsKey(name) ? default : Parameters[name] as T;
    }

    public void SetParameter(string name, object value)
    {
        if (Parameters.ContainsKey(name))
            Parameters[name] = value;
        else
            Parameters.Add(name, value);
    }
}
