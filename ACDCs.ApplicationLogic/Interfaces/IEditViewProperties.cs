namespace ACDCs.ApplicationLogic.Interfaces;

using Components.Circuit;
using Sharp.UI;

[BindableProperties]
public interface IEditViewProperties
{
    double ButtonHeight { get; set; }
    double ButtonWidth { get; set; }
    CircuitView CircuitView { get; set; }
}
