using ACDCs.Data.ACDCs.Components;
using Sharp.UI;

namespace ACDCs.Views.ModelSelection;

[BindableProperties]
public class ComponentViewModel
{
    public Color ItemBackground { get; set; }
    public IElectronicComponent Model { get; set; }
    public string Name { get; set; }
    public int Row { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
}
