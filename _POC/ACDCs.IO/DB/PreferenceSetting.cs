namespace ACDCs.IO.DB;

public class PreferenceSetting
{
    private object? _value;
    public string? Description { get; set; }
    public string? Group { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Key { get; set; }
    public string? TypeName { get; set; }

    public object? Value
    {
        get
        {
            if (TypeName == null)
            {
                return _value;
            }

            Type? valueType = Type.GetType(TypeName);
            return valueType != null ? Convert.ChangeType(_value, valueType) : _value;
        }
        set
        {
            _value = value;
        }
    }
}
