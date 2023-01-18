using ACDCs.Views;
using ACDCs.Views.Circuit;
using Sharp.UI;
using AbsoluteLayout = Sharp.UI.AbsoluteLayout;

namespace ACDCs.Interfaces;

[BindableProperties]
public interface IMenuViewProperties
{
    CircuitView CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    string MenuFilename { get; set; }
    AbsoluteLayout PopupTarget { get; set; }
}
