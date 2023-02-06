namespace ACDCs.ApplicationLogic.Interfaces;

using Components;
using Components.Circuit;
using Sharp.UI;

[BindableProperties]
public interface IMenuHandlerProperties
{
    CircuitView CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    Page PopupPage { get; set; }
}
