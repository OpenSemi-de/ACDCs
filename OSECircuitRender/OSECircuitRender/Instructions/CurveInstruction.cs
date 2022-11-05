using OSECircuitRender.Definitions;

namespace OSECircuitRender.Instructions;

public sealed class CurveInstruction : DrawInstruction
{
    public Coordinate End { get; set; }

    public CurveInstruction(float x1, float y1, float x2, float y2, float angleStart, float angleEnd) : base(typeof(CurveInstruction))
    {
        Position = new(x1, y1, 0);
        End = new Coordinate(x2, y2, 0);
        AngleStart = angleStart;
        AngleEnd = angleEnd;
        StrokeColor = new Color(0, 0, 0);
    }

    public float AngleEnd { get; set; }

    public float AngleStart { get; set; }
}