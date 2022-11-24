using System.Threading.Tasks;
using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;
using OSECircuitRender.Scene;

namespace OSECircuitRender.Interfaces;

public interface ISceneManager
{
    Color BackgroundColor { get; set; }
    Coordinate DisplayOffset { get; set; }
    SheetScene Scene { get; set; }
    bool ShowGrid { get; set; }

    object GetSceneForBackend();

    bool SendToBackend(object backendScene);

    bool SetScene(DrawableComponentList drawables, DrawableComponentList selected);

    void SetSizeAndScale(Coordinate sheetSize, float gridSize);
}