using ACDCs.ApplicationLogic.Views;
using CircuitView = ACDCs.ApplicationLogic.Components.Circuit.CircuitView;

namespace ACDCs.ApplicationLogic.Interfaces;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

public interface IMenuViewProperties
{
    CircuitView? CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    string MenuFilename { get; set; }
    AbsoluteLayout PopupTarget { get; set; }
}
