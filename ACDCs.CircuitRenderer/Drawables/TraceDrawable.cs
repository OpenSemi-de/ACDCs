using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;

namespace ACDCs.CircuitRenderer.Drawables;

public class TraceDrawable : DrawableComponent
{
    public TraceDrawable(IWorksheetItem parent) : base(typeof(TraceDrawable), parent)
    {
        SetSize(1, 1);
        SetPosition(0, 0);
    }

    public void AddPart(Coordinate from, Coordinate to)
    {
        DrawInstructions.Add(
            new LineInstruction(from.X, from.Y, to.X, to.Y, 3)
        );
    }
}
