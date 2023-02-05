namespace ACDCs.ApplicationLogic.Interfaces;

using Components.Circuit;
using Sharp.UI;
using Views;

[BindableProperties]
public interface IMenuHandlerProperties
{
    CircuitView CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    Page PopupPage { get; set; }
}
