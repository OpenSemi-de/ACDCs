using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;
using OSECircuitRender.Scene;

namespace OSECircuitRender.Interfaces;

public interface ISceneManager
{
    SheetScene Scene { get; set; }

    object GetSceneForBackend();

    bool SendToBackend(object backendScene);

    bool SetScene(DrawableComponentList drawables, DrawableComponentList selected);

    void SetSizeAndScale(Coordinate sheetSize, float gridSize);
}