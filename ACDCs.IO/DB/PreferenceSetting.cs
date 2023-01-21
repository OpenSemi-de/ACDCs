namespace ACDCs.IO.DB;

public class PreferenceSetting<T> : IPreferenceSetting
{
    public string Key { get; }

    public object ObjectValue
    {
        get => Value;
    }

    public T Value { get; }

    public PreferenceSetting(T value, string key)
    {
        Value = value;
        Key = key;
    }
}

public interface IPreferenceSetting
{
    string Key { get; }
    object ObjectValue { get; }
}
