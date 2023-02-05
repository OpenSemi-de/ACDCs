namespace ACDCs.ApplicationLogic.Interfaces;

using Components.Circuit;
using Sharp.UI;
using Views;

public interface IMenuViewProperties
{
    CircuitView? CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    string MenuFilename { get; set; }
    AbsoluteLayout PopupTarget { get; set; }
}
