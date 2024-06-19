namespace ACDCs.API.Windowing.Components.Menu;

public class MenuHandler
{
    private Dictionary<string, object> Parameters { get; } = new();

    public void SetParameter(string name, object value)
    {
        if (Parameters.ContainsKey(name))
            Parameters[name] = value;
        else
            Parameters.Add(name, value);
    }

    protected T? GetParameter<T>(string name) where T : class
    {
        return !Parameters.ContainsKey(name) ? default : Parameters[name] as T;
    }
}
