using Sharp.UI;

namespace ACDCs.Interfaces;

[BindableProperties]
public interface IWindowViewProperties
{
    public View WindowContent { get; set; }
}
