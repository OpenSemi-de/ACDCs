using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Scene;

namespace ACDCs.CircuitRenderer.Interfaces;

public interface ISceneManager
{
    Color? BackgroundColor { get; set; }
    Coordinate? DisplayOffset { get; set; }
    SheetScene? Scene { get; set; }
    bool ShowGrid { get; set; }
    Color? ForegroundColor { get; set; }
    Color? BackgroundHighColor { get; set; }
    object? GetSceneForBackend();
    bool SendToBackend(object? backendScene);
    bool SetScene(DrawableComponentList drawables, DrawableComponentList selected, PinDrawable? selectedPin);
    void SetSizeAndScale(Coordinate sheetSize, float gridSize);
}
