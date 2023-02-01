using ACDCs.ApplicationLogic.Components.Circuit;
using ACDCs.ApplicationLogic.Views;

namespace ACDCs.ApplicationLogic.Interfaces;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

[BindableProperties]
public interface IMenuHandlerProperties
{
    CircuitView CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    Page PopupPage { get; set; }
}
