using Sharp.UI;

namespace ACDCs.ApplicationLogic.Interfaces;

[BindableProperties]
public interface IWindowViewProperties
{
    public View WindowContent { get; set; }
}
