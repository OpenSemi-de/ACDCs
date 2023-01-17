using ACDCs.Views.Circuit;

namespace ACDCs.Views.Edit;

using Sharp.UI;

[BindableProperties]
public interface IEditViewProperties
{
    double ButtonHeight { get; set; }
    double ButtonWidth { get; set; }
    CircuitView CircuitView { get; set; }
}
