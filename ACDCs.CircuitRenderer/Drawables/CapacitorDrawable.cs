using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Newtonsoft.Json;

namespace ACDCs.CircuitRenderer.Drawables;

public sealed class CapacitorDrawable : DrawableComponent
{
    public CapacitorDrawableType CapacitorType { get; set; }
    public TextInstruction? TextInstruction { get; private set; }

    [JsonConstructor]
    public CapacitorDrawable() : base(typeof(CapacitorDrawable), null)
    {
        Setup();
    }

    public CapacitorDrawable(IWorksheetItem parent, string value, CapacitorDrawableType type, float x, float y) : base(typeof(CapacitorDrawable), parent)
    {
        Initialize(value, type);
        Setup(value, type, x, y);
    }

    private void Initialize(string value, CapacitorDrawableType capacitorType)
    {
        CapacitorType = capacitorType;
        DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.3f, 0.5f));

        switch (CapacitorType)
        {
            case CapacitorDrawableType.Standard:
                DrawablePins.Add(new PinDrawable(ParentItem, 0, 0.5f));
                DrawablePins.Add(new PinDrawable(ParentItem, 1f, 0.5f));

                DrawInstructions.Add(new BoxInstruction(0.3f, 0.1f, 0.15f, 0.8f));
                DrawInstructions.Add(new BoxInstruction(0.55f, 0.1f, 0.15f, 0.8f));
                DrawInstructions.Add(new LineInstruction(0.7f, 0.5f, 1f, 0.5f));
                break;

            case CapacitorDrawableType.Polarized:
                DrawablePins.Add(new PinDrawable(ParentItem, 0, 0.5f, "+"));
                DrawablePins.Add(new PinDrawable(ParentItem, 1f, 0.5f, "-"));
                DrawInstructions.Add(new BoxInstruction(0.3f, 0.1f, 0.1f, 0.8f, new Color(0, 0, 0)));
                DrawInstructions.Add(new CurveInstruction(0.5f, 0.1f, 0.8f, 0.9f, 90f, 270f));
                DrawInstructions.Add(new LineInstruction(0.5f, 0.5f, 1f, 0.5f));
                break;
        }

        TextInstruction = new TextInstruction(value, 0f, 12f, 0.5f, 1.35f);
        DrawInstructions.Add(TextInstruction);
    }

    private void Setup(string value = "N/A", CapacitorDrawableType capacitorType = CapacitorDrawableType.Standard, float x = 0, float y = 0)
    {
        SetSize(2, 1);
        SetPosition(x, y);

        OnValueSet = ValueSet;
    }

    private void ValueSet()
    {
        if (TextInstruction != null)
        {
            TextInstruction.Text = Value;
        }
    }
}
