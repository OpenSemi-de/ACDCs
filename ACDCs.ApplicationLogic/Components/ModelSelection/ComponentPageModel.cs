using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.ApplicationLogic.Components.ModelSelection;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

[BindableProperties]
public class ComponentViewModel
{
    public Color ItemBackground { get; set; } = Colors.Transparent;
    public IElectronicComponent? Model { get; set; }
    public string? Name { get; set; } = string.Empty;
    public int Row { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
