namespace ACDCs.Components.Properties;

public class PropertyItem
{
    public List<PropertyItem> Children { get; set; } = new();
    public bool IsExpanded { get; set; }
    public bool IsLeaf { get; set; }
    public string Name { get; set; } = string.Empty;
    public Type ParentType { get; set; }
    public object? Value { get; set; } = null;

    public PropertyItem(string? name)
    {
        if (name != null)
        {
            Name = name;
        }
    }
}
