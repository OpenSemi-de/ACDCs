using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;

namespace OSECircuitRender.Items;

public class TraceItem : WorksheetItem
{
    public TraceItem()
    {
        DrawableComponent = new TraceDrawable(this);
    }

    public void AddPart(Coordinate from, Coordinate to)
    {
        ((TraceDrawable)DrawableComponent).AddPart(from, to);
    }
}