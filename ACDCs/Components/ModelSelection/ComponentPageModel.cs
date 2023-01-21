using ACDCs.Data.ACDCs.Interfaces;
using Sharp.UI;

namespace ACDCs.Components.ModelSelection;

[BindableProperties]
public class ComponentViewModel
{
    public Color ItemBackground { get; set; } = Colors.Transparent;
    public IElectronicComponent? Model { get; set; }
    public string? Name { get; set; } = string.Empty;
    public int Row { get; set; } = 0;
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
