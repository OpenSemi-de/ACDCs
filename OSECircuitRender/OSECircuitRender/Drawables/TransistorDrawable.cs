using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Drawables;

public sealed class TransistorDrawable : DrawableComponent
{
    public TransistorDrawable() : base(typeof(TransistorDrawable))
    {
        Setup(null);
    }

    public TransistorDrawable(IWorksheetItem backRef) : base(typeof(TransistorDrawable))
    {
        Setup(backRef);
    }

    public TransistorDrawable(IWorksheetItem backRef, TransistorDrawableType type, float x, float y) : base(typeof(TransistorDrawable))
    {
        Setup(backRef, type, x, y);
    }

    public TransistorDrawable(TransistorDrawableType type, float x, float y) : base(typeof(TransistorDrawable))
    {
        Setup(null, type, x, y);
    }

    private void Setup(IWorksheetItem backRef, TransistorDrawableType type = TransistorDrawableType.PNP, float x = 0, float y = 0)
    {
        DrawablePins.Add(new PinDrawable(backRef, 0, 0.5f));
        DrawablePins.Add(new PinDrawable(backRef, 1f, 0f));
        DrawablePins.Add(new PinDrawable(backRef, 1f, 1f));
        DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.5f, 0.5f));
        DrawInstructions.Add(new LineInstruction(0.5f, 0.5f, 1f, 0.2f));
        DrawInstructions.Add(new LineInstruction(1f, 0f, 1f, 0.2f));
        DrawInstructions.Add(new LineInstruction(1f, 0.8f, 1f, 1f));
        DrawInstructions.Add(new LineInstruction(0.5f, 0.5f, 1f, 0.8f));
        DrawInstructions.Add(new LineInstruction(0.5f, 0.2f, 0.5f, 0.8f));
        DrawInstructions.Add(new TextInstruction(type.ToString(), 0f, 12f, 0.5f, 1.3f));
        SetSize(1, 2);
        SetPosition(x, y);
        SetRef(backRef);
    }
}