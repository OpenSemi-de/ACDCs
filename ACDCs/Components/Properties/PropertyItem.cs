namespace ACDCs.Components.Properties;

public class PropertyItem
{
    public string Name { get; set; } = "";

    public string StringValue
    {
        get
        {
            if (Value != null)
                return Convert.ToString(Value) ?? string.Empty;
            return string.Empty;
        }
    }

    public object? Value { get; set; } = null;
}
