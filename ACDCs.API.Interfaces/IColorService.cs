namespace ACDCs.API.Interfaces;

public interface IColorService
{
    Color Background { get; }
    Color BackgroundHigh { get; }
    Color Border { get; }
    Color Foreground { get; }
    Color Full { get; }
    Color Text { get; }

    Color GetColor(string colorName);
}
