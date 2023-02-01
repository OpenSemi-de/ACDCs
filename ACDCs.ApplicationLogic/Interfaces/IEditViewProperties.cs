using Sharp.UI;
using CircuitView = ACDCs.ApplicationLogic.Components.Circuit.CircuitView;

namespace ACDCs.ApplicationLogic.Interfaces;

[BindableProperties]
public interface IEditViewProperties
{
    double ButtonHeight { get; set; }
    double ButtonWidth { get; set; }
    CircuitView CircuitView { get; set; }
}
