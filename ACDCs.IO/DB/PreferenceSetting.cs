namespace ACDCs.IO.DB;

public class PreferenceSetting
{
    private object? _value;
    public string? Description { get; set; }
    public string? Group { get; set; }
    public string? Key { get; set; }
    public string? TypeName { get; set; }

    public object? Value
    {
        get
        {
            if (TypeName != null)
            {
                Type? valueType = Type.GetType(TypeName);
                if (valueType != null)
                {
                    return Convert.ChangeType(_value, valueType);
                }
            }
            return _value;
        }
        set
        {
            _value = value;
        }
    }
}
