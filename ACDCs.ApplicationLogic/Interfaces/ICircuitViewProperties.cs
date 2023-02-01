using Sharp.UI;
using AbsoluteLayout = Sharp.UI.AbsoluteLayout;

namespace ACDCs.ApplicationLogic.Interfaces;

[BindableProperties]
public interface ICircuitViewProperties
{
    public Color BackgroundHighColor { get; set; }
    public Color ForegroundColor { get; set; }
    public AbsoluteLayout PopupTarget { get; set; }
}
