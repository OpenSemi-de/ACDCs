namespace ACDCs.API.Interfaces;

using Sharp.UI;

[BindableProperties]
public interface ICircuitViewProperties
{
    public Color BackgroundHighColor { get; set; }
    public Color ForegroundColor { get; set; }
}
