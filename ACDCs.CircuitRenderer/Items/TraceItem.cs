using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items;

public class TraceItem : WorksheetItem
{
    public TraceItem()
    {
        DrawableComponent = new TraceDrawable(this);
    }

    public override string DefaultValue => "";

    public override bool IsInsertable => false;

    public void AddPart(Coordinate from, Coordinate to)
    {
        ((TraceDrawable)DrawableComponent).AddPart(from, to);
    }
}
