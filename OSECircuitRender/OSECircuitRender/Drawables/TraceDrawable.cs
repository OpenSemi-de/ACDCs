using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Drawables;

public class TraceDrawable : DrawableComponent
{
    public TraceDrawable(IWorksheetItem? backRef) : base(typeof(TraceDrawable))
    {
        //BackRef = backRef;
        SetSize(1, 1);
        SetPosition(0, 0);
    }

    public void AddPart(Coordinate from, Coordinate to)
    {
        DrawInstructions.Add(
            new LineInstruction(from.X, from.Y, to.X, to.Y)
        );
    }
}