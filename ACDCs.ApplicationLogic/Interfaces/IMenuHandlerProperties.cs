namespace ACDCs.ApplicationLogic.Interfaces;

using Components.Circuit;
using Components.Components;
using Sharp.UI;

[BindableProperties]
public interface IMenuHandlerProperties
{
    CircuitView CircuitView { get; set; }
    ComponentsView ComponentsView { get; set; }
    Page PopupPage { get; set; }
}
