using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Drawables;

public sealed class ResistorDrawable : DrawableComponent
{
    public ResistorDrawable(IWorksheetItem parent, string value, float x, float y) : base(typeof(ResistorDrawable), parent)
    {
        Setup(value, x, y);
    }

    private void Setup(string value = "N/A", float x = 0, float y = 0)
    {
        DrawablePins.Add(new PinDrawable(ParentItem, 0, 0.5f));
        DrawablePins.Add(new PinDrawable(ParentItem, 1f, 0.5f));
        DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.2f, 0.5f));
        DrawInstructions.Add(new BoxInstruction(0.2f, 0.35f, 0.6f, 0.3f));
        DrawInstructions.Add(new LineInstruction(0.8f, 0.5f, 1f, 0.5f));
        DrawInstructions.Add(new TextInstruction(value, 0f, 12f, 0.5f, 1.35f));
        SetSize(2, 1);
        SetPosition(x, y);
    }
}
