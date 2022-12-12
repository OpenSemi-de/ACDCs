using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;

namespace ACDCs.CircuitRenderer.Scene;

public sealed class SheetScene
{
    public SheetScene(ISceneManager sceneManager)
    {
        DisplayOffset = new Coordinate(0, 0, 0);
        SheetSize = new Coordinate(100, 100, 0);
        SceneManager = sceneManager;
    }

    public Color? BackgroundColor { get; set; }
    public Color? BackgroundHighColor { get; set; }
    public Coordinate? DisplayOffset { get; set; }
    public DrawableComponentList? Drawables { get; set; }
    public Color? ForegroundColor { get; set; }
    public float GridSize { get; set; }
    public DrawableComponentList? Selected { get; set; }
    public PinDrawable? SelectedPin { get; set; }
    public Coordinate SheetSize { get; set; }
    public bool ShowGrid { get; set; } = true;
    public ISceneManager SceneManager { get; set; }

    public bool IsSelected(IDrawableComponent drawable)
    {
        return Selected != null && Selected.Contains(drawable);
    }

    public bool SetDrawables(DrawableComponentList drawables, DrawableComponentList selected)
    {
        Drawables = drawables;
        Selected = selected;
        return true;
    }
}
