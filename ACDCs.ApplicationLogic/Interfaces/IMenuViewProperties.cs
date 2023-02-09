namespace ACDCs.ApplicationLogic.Interfaces;

using Components.Circuit;
using Components.Components;
using Sharp.UI;

public interface IMenuViewProperties
{
    CircuitView? CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    string MenuFilename { get; set; }
    AbsoluteLayout PopupTarget { get; set; }
}
