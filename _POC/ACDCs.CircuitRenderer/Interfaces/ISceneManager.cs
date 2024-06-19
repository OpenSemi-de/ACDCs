using System.Collections.Generic;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Scene;

namespace ACDCs.CircuitRenderer.Interfaces;

public interface ISceneManager
{
    Color? BackgroundColor { get; set; }
    Color? BackgroundHighColor { get; set; }
    short[,] CollisionMap { get; set; }
    List<RectFr> DebugRects { get; set; }
    Coordinate? DisplayOffset { get; set; }
    List<FeedbackRect>? FeedbackRects { get; }
    Color? ForegroundColor { get; set; }
    SheetScene? Scene { get; set; }
    bool ShowCollisionMap { get; set; }
    bool ShowGrid { get; set; }

    object? GetSceneForBackend();

    void PutFeedbackRect(bool isSelected, DrawableComponent? drawable, Coordinate drawPos, Coordinate drawSize,
        Coordinate? displayOffset);

    bool SendToBackend(object? backendScene);

    bool SetScene(DrawableComponentList drawables, DrawableComponentList selected, PinDrawable? selectedPin);

    void SetSizeAndScale(Coordinate sheetSize, float gridSize);
}
