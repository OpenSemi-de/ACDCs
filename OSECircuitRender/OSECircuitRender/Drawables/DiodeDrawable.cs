using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Drawables;

public sealed class DiodeDrawable : DrawableComponent
{
    public DiodeDrawable() : base(typeof(DiodeDrawable))
    {
        Setup(null);
    }

    public DiodeDrawable(IWorksheetItem? backRef) : base(typeof(DiodeDrawable))
    {
        Setup(backRef);
    }

    public DiodeDrawable(IWorksheetItem? backRef, string value, float x, float y) : base(typeof(DiodeDrawable))
    {
        Setup(backRef, value, x, y);
    }

    public DiodeDrawable(string value, float x, float y) : base(typeof(DiodeDrawable))
    {
        Setup(null, value, x, y);
    }

    private void Setup(IWorksheetItem? backRef, string value = "N/A", float x = 0, float y = 0)
    {
        DrawablePins.Add(new PinDrawable(backRef, 0, 0.5f));
        DrawablePins.Add(new PinDrawable(backRef, 1f, 0.5f));
        DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.5f, 0.5f));
        DrawInstructions.Add(new LineInstruction(0.8f, 0.5f, 0.5f, 0.2f));
        DrawInstructions.Add(new LineInstruction(0.8f, 0.5f, 0.5f, 0.8f));
        DrawInstructions.Add(new LineInstruction(0.5f, 0.2f, 0.5f, 0.8f));
        DrawInstructions.Add(new LineInstruction(0.8f, 0.5f, 1f, 0.5f));
        DrawInstructions.Add(new TextInstruction(value, 0f, 12f, 0.5f, 1.3f));
        SetSize(2, 1);
        SetPosition(x, y);
        SetRef(backRef);
    }
}