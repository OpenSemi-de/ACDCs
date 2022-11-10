using OSECircuitRender.Definitions;
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
        DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.5f, 0.5f));
        DrawInstructions.Add(new LineInstruction(0.5f, 0.5f, 1f, 0.2f));
        DrawInstructions.Add(new LineInstruction(1f, 0f, 1f, 0.2f));
        DrawInstructions.Add(new LineInstruction(1f, 0.8f, 1f, 1f));
        DrawInstructions.Add(new LineInstruction(0.5f, 0.5f, 1f, 0.8f));
        DrawInstructions.Add(new BoxInstruction(0.5f, 0.3f, 0.05f, 0.4f, new Color(0, 0, 0)));
        DrawInstructions.Add(new TextInstruction(type.ToString(), 0f, 12f, 0.3f, 1f));

        if (type == TransistorDrawableType.PNP)
        {
            DrawablePins.Add(new PinDrawable(backRef, 0, 0.5f, pinText: "B"));
            DrawablePins.Add(new PinDrawable(backRef, 1f, 0f, pinText: "E"));
            DrawablePins.Add(new PinDrawable(backRef, 1f, 1f, pinText: "C"));

            DrawInstructions.Add(new LineInstruction(0.7f, 0.4f, 0.7f, 0.25f));
            DrawInstructions.Add(new LineInstruction(0.7f, 0.4f, 0.85f, 0.4f));
            DrawInstructions.Add(new LineInstruction(0.7f, 0.25f, 0.85f, 0.4f));
        }

        if (type == TransistorDrawableType.NPN)
        {
            DrawablePins.Add(new PinDrawable(backRef, 0, 0.5f, pinText: "B"));
            DrawablePins.Add(new PinDrawable(backRef, 1f, 0f, pinText: "C"));
            DrawablePins.Add(new PinDrawable(backRef, 1f, 1f, pinText: "E"));

            DrawInstructions.Add(new LineInstruction(0.8f, 0.7f, 0.8f, 0.55f));
            DrawInstructions.Add(new LineInstruction(0.8f, 0.7f, 0.65f, 0.7f));
            DrawInstructions.Add(new LineInstruction(0.65f, 0.7f, 0.8f, 0.55f));
        }

        SetSize(2, 2);
        SetPosition(x, y);
        SetRef(backRef);
    }
}