using ACDCs.Views;
using Sharp.UI;
using AbsoluteLayout = Sharp.UI.AbsoluteLayout;
using CircuitView = ACDCs.Components.Circuit.CircuitView;

namespace ACDCs.Interfaces;

[BindableProperties]
public interface IMenuViewProperties
{
    CircuitView CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    string MenuFilename { get; set; }
    AbsoluteLayout PopupTarget { get; set; }
}
