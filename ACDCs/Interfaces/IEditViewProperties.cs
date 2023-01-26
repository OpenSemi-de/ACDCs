using Sharp.UI;
using CircuitView = ACDCs.Components.Circuit.CircuitView;

namespace ACDCs.Interfaces;

[BindableProperties]
public interface IEditViewProperties
{
    double ButtonHeight { get; set; }
    double ButtonWidth { get; set; }
    CircuitView CircuitView { get; set; }
}
