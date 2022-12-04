using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Drawables;

public class PinDrawable : DrawableComponent
{
    public PinDrawable(IWorksheetItem? backRef, float x, float y, string pinText = "") : base(typeof(PinDrawable))
    {
        PinText = pinText;
        DrawInstructions.Add(new CircleInstruction(0, 0, 1, 1));
        DrawInstructions.Add(new TextInstruction(pinText, 0, 12, 0.5f, 1.2f));
        Setup(backRef, x, y);
    }

    public PinDrawable(PinDrawable pin) : base(typeof(PinDrawable))
    {
        PinText = pin.PinText;
        Position = new Coordinate(Position);
        Size = new Coordinate(Size);
        //BackRef = pin.BackRef;
        Setup(BackRef, 1, 1);
    }

    public string PinText { get; }

    private void Setup(IWorksheetItem? backRef, float x, float y)
    {
        if (backRef != null && !backRef.Pins.Contains(this))
            backRef.Pins.Add(this);
        SetSize(1, 1);
        SetPosition(x, y);
        SetRef(backRef);
    }
}