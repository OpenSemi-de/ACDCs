using ACDCs.Views.Circuit;
using Sharp.UI;

namespace ACDCs.Interfaces;

[BindableProperties]
public interface IEditViewProperties
{
    double ButtonHeight { get; set; }
    double ButtonWidth { get; set; }
    CircuitView CircuitView { get; set; }
}
