namespace ACDCs.API.Interfaces;

using Sharp.UI;

public interface IMenuViewProperties
{
    ICircuitView? CircuitView { get; set; }
    IComponentsView ComponentsView { get; set; }
    string MenuFilename { get; set; }
    AbsoluteLayout PopupTarget { get; set; }
}
