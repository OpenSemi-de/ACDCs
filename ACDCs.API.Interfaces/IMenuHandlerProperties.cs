namespace ACDCs.API.Interfaces;

using Sharp.UI;

[BindableProperties]
public interface IMenuHandlerProperties
{
    ICircuitView CircuitView { get; set; }
    IComponentsView ComponentsView { get; set; }
    Page PopupPage { get; set; }
}
