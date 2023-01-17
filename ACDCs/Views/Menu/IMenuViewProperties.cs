using ACDCs.Views.Circuit;

namespace ACDCs.Views.Menu;

using Sharp.UI;

[BindableProperties]
public interface IMenuViewProperties
{
    CircuitView CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    string MenuFilename { get; set; }
    AbsoluteLayout PopupTarget { get; set; }
}
