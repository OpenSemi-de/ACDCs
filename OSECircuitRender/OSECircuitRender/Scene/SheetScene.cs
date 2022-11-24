using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Scene;

public sealed class SheetScene
{
    public Color BackgroundColor { get; set; }
    public Coordinate DisplayOffset { get; set; }
    public DrawableComponentList Drawables { get; set; }
    public float GridSize { get; set; }
    public DrawableComponentList Selected { get; set; }
    public Coordinate SheetSize { get; set; }
    public bool ShowGrid { get; set; } = true;

    public bool IsSelected(IDrawableComponent drawable)
    {
        return Selected.Contains(drawable);
    }

    public bool SetDrawables(DrawableComponentList drawables, DrawableComponentList selected)
    {
        Drawables = drawables;
        Selected = selected;
        return true;
    }
}