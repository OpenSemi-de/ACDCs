using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Scene;

public sealed class DefaultSceneManager : ISceneManager
{
    public object DrawableScene { get; set; }
    public SheetScene Scene { get; set; }
    public bool ShowGrid { get; set; } = true;

    public Coordinate DisplayOffset { get; set; }

    public object GetSceneForBackend()
    {
        DrawableScene drawableScene = new(Scene);
        drawableScene.DisplayOffset = DisplayOffset;
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
        Scene.ShowGrid = ShowGrid;
        Scene.BackgroundColor = BackgroundColor;
        Scene.DisplayOffset = DisplayOffset;
        return true;
    }

    public Color BackgroundColor { get; set; }

    public void SetSizeAndScale(Coordinate sheetSize, float gridSize)
    {
        Scene.GridSize = gridSize;
        Scene.SheetSize = sheetSize;
    }
}