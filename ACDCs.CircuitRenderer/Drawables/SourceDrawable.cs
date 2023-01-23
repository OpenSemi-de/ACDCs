using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Items;

namespace ACDCs.CircuitRenderer.Drawables;

public class SourceDrawable : DrawableComponent
{
    public static string DefaultValue { get; set; } = "5v";

    public SourceDrawableType SourceType { get; set; }

    public SourceDrawable(WorksheetItem parent, SourceDrawableType type) : base(typeof(SourceDrawable), parent)
    {
        Setup(DefaultValue, type);
    }

    public SourceDrawable(WorksheetItem parent, string value, SourceDrawableType type, float x, float y) : base(typeof(SourceDrawable), parent)
    {
        Value = value;
        Setup(value, type, x, y);
    }

    private void Setup(string value = "N/A", SourceDrawableType type = SourceDrawableType.Voltage, float x = 0, float y = 0)
    {
        SourceType = type;
        DrawablePins.Add(new PinDrawable(ParentItem, 0, 0.5f));
        DrawablePins.Add(new PinDrawable(ParentItem, 1f, 0.5f));
        DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.25f, 0.5f));
        DrawInstructions.Add(new LineInstruction(0.25f, 0.2f, 0.25f, 0.8f));
        DrawInstructions.Add(new LineInstruction(0.45f, 0.3f, 0.45f, 0.7f));
        DrawInstructions.Add(new LineInstruction(0.55f, 0.2f, 0.55f, 0.8f));
        DrawInstructions.Add(new LineInstruction(0.75f, 0.3f, 0.75f, 0.7f));
        DrawInstructions.Add(new LineInstruction(0.75f, 0.5f, 1f, 0.5f));
        DrawInstructions.Add(new TextInstruction(value, 0f, 12f, 0.5f, 1.35f));
        SetSize(2, 2);
        SetPosition(x, y);
    }
}
