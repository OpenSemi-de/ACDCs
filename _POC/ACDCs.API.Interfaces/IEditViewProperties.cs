namespace ACDCs.API.Interfaces;

using Sharp.UI;

[BindableProperties]
public interface IEditViewProperties
{
    double ButtonHeight { get; set; }
    double ButtonWidth { get; set; }
    ICircuitView CircuitView { get; set; }
}
