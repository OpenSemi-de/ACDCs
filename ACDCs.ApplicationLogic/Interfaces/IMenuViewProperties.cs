namespace ACDCs.ApplicationLogic.Interfaces;

using Components;
using Components.Circuit;
using Sharp.UI;

public interface IMenuViewProperties
{
    CircuitView? CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    string MenuFilename { get; set; }
    AbsoluteLayout PopupTarget { get; set; }
}
