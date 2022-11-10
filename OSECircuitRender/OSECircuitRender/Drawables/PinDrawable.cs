using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Drawables;

public class PinDrawable : DrawableComponent
{
    public string PinText { get; }

    public PinDrawable(IWorksheetItem backRef, float x, float y, string pinText = "") : base(typeof(PinDrawable))
    {
        PinText = pinText;
        DrawInstructions.Add(new CircleInstruction(0, 0, 1, 1));
        DrawInstructions.Add(new TextInstruction(pinText, 0, 12, 0.5f, 1.2f));
        Setup(backRef, x, y);
    }

    private void Setup(IWorksheetItem backRef, float x, float y)
    {
        if (!backRef.Pins.Contains(this))
            backRef?.Pins.Add(this);
        SetSize(1, 1);
        SetPosition(x, y);
        SetRef(backRef);
    }
}