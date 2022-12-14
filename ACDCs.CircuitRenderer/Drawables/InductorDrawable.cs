using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;

namespace ACDCs.CircuitRenderer.Drawables;

public sealed class InductorDrawable : DrawableComponent
{
    public InductorDrawable(IWorksheetItem parent, string value, float x, float y) : base(typeof(ResistorDrawable), parent)
    {
        Setup(value, x, y);
    }

    private void Setup(string value = "N/A", float x = 0, float y = 0)
    {
        DrawablePins.Add(new PinDrawable(ParentItem, 0f, 0.5f));
        DrawablePins.Add(new PinDrawable(ParentItem, 1f, 0.5f));
        DrawInstructions.Add(new PathInstruction(
            "M 0,8.5 L 6.5,8.5 C 6.5,8.5 6.5,4.5 10.5,4.5 C 14.5,4.5 14.5,8.5 14.5,8.5 C 14.5,8.5 14.5,4.5 18.5,4.5 C 22.5,4.5 22.5,8.5 22.5,8.5 C 22.5,8.5 22.5,4.5 26.5,4.5 C 30.5,4.5 30.5,8.5 30.5,8.5 C 30.5,8.5 30.5,4.5 34.5,4.5 C 38.5,4.5 38.5,8.5 38.5,8.5 L 45,8.5"));
        DrawInstructions.Add(new TextInstruction(value, 0f, 12f, 0.5f, 1.35f));

        SetSize(2, 1);
        SetPosition(x, y);
    }
}
