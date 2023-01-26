using ACDCs.Views;
using CircuitView = ACDCs.Components.Circuit.CircuitView;

namespace ACDCs.Interfaces;

using Sharp.UI;

[BindableProperties]
public interface IMenuHandlerProperties
{
    CircuitView CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    Page PopupPage { get; set; }
}
