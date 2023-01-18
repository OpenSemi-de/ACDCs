using ACDCs.Views;
using ACDCs.Views.Circuit;

namespace ACDCs.Interfaces;

using Sharp.UI;

[BindableProperties]
public interface IMenuHandlerViewProperties
{
    CircuitView CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    Page PopupPage { get; set; }
}
