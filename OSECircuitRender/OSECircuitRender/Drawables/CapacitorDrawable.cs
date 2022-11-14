using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Drawables;

public sealed class CapacitorDrawable : DrawableComponent
{
    public CapacitorDrawable() : base(typeof(CapacitorDrawable))
    {
        Setup(null);
    }

    public CapacitorDrawable(IWorksheetItem backRef) : base(typeof(CapacitorDrawable))
    {
        Setup(backRef);
    }

    public CapacitorDrawable(IWorksheetItem backRef, string value, CapacitorDrawableType type, float x, float y) : base(
        typeof(CapacitorDrawable))
    {
        Setup(backRef, value, type, x, y);
    }

    public CapacitorDrawableType CapacitorType { get; set; }

    private void Setup(IWorksheetItem backRef, string value = "N/A",
        CapacitorDrawableType capacitorType = CapacitorDrawableType.Standard, float x = 0, float y = 0)
    {
        CapacitorType = capacitorType;
        DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.3f, 0.5f));

        if (CapacitorType == CapacitorDrawableType.Standard)
        {
            DrawablePins.Add(new PinDrawable(backRef, 0, 0.5f));
            DrawablePins.Add(new PinDrawable(backRef, 1f, 0.5f));

            DrawInstructions.Add(new BoxInstruction(0.3f, 0.1f, 0.15f, 0.8f));
            DrawInstructions.Add(new BoxInstruction(0.55f, 0.1f, 0.15f, 0.8f));
            DrawInstructions.Add(new LineInstruction(0.7f, 0.5f, 1f, 0.5f));
        }

        if (CapacitorType == CapacitorDrawableType.Polarized)
        {
            DrawablePins.Add(new PinDrawable(backRef, 0, 0.5f, "+"));
            DrawablePins.Add(new PinDrawable(backRef, 1f, 0.5f, "-"));
            DrawInstructions.Add(new BoxInstruction(0.3f, 0.1f, 0.1f, 0.8f, new Color(0, 0, 0)));
            DrawInstructions.Add(new CurveInstruction(0.5f, 0.1f, 0.8f, 0.9f, 90f, 270f));
            DrawInstructions.Add(new LineInstruction(0.5f, 0.5f, 1f, 0.5f));
        }

        DrawInstructions.Add(new TextInstruction(value, 0f, 12f, 0.5f, 1.35f));
        SetSize(2, 1);
        SetPosition(x, y);
        SetRef(backRef);
    }
}