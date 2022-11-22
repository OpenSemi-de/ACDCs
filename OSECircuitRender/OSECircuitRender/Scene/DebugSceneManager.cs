using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Scene;

public sealed class DebugSceneManager : ISceneManager
{
    public object DrawableScene { get; set; }
    public SheetScene Scene { get; set; }

    public object GetSceneForBackend()
    {
        DrawableScene drawableScene = new(Scene);
        DrawableScene = drawableScene;
        return drawableScene;
    }

    public bool SendToBackend(object backendScene)
    {
        return true;
    }

    public bool SetScene(DrawableComponentList drawables, DrawableComponentList selected)
    {
        Scene = new SheetScene();
        Scene.SetDrawables(drawables, selected);
        Scene.ShowGrid = true;
        return true;
    }

    public void SetSizeAndScale(Coordinate sheetSize, float gridSize)
    {
        Scene.GridSize = gridSize;
        Scene.SheetSize = sheetSize;
    }
}