using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;

namespace ACDCs.CircuitRenderer.Drawables;

public sealed class CapacitorDrawable : DrawableComponent
{
    private TextInstruction? _textInstruction;
    public CapacitorDrawableType CapacitorType { get; set; }

    public CapacitorDrawable(IWorksheetItem parent) : base(typeof(CapacitorDrawable), parent)
    {
        Setup();
    }

    public CapacitorDrawable(IWorksheetItem parent, string value, CapacitorDrawableType type, float x, float y) : base(typeof(CapacitorDrawable), parent)
    {
        Setup(value, type, x, y);
    }

    private void Setup(string value = "N/A", CapacitorDrawableType capacitorType = CapacitorDrawableType.Standard, float x = 0, float y = 0)
    {
        CapacitorType = capacitorType;
        DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.3f, 0.5f));

        if (CapacitorType == CapacitorDrawableType.Standard)
        {
            DrawablePins.Add(new PinDrawable(ParentItem, 0, 0.5f));
            DrawablePins.Add(new PinDrawable(ParentItem, 1f, 0.5f));

            DrawInstructions.Add(new BoxInstruction(0.3f, 0.1f, 0.15f, 0.8f));
            DrawInstructions.Add(new BoxInstruction(0.55f, 0.1f, 0.15f, 0.8f));
            DrawInstructions.Add(new LineInstruction(0.7f, 0.5f, 1f, 0.5f));
        }

        if (CapacitorType == CapacitorDrawableType.Polarized)
        {
            DrawablePins.Add(new PinDrawable(ParentItem, 0, 0.5f, "+"));
            DrawablePins.Add(new PinDrawable(ParentItem, 1f, 0.5f, "-"));
            DrawInstructions.Add(new BoxInstruction(0.3f, 0.1f, 0.1f, 0.8f, new Color(0, 0, 0)));
            DrawInstructions.Add(new CurveInstruction(0.5f, 0.1f, 0.8f, 0.9f, 90f, 270f));
            DrawInstructions.Add(new LineInstruction(0.5f, 0.5f, 1f, 0.5f));
        }

        _textInstruction = new TextInstruction(value, 0f, 12f, 0.5f, 1.35f);
        DrawInstructions.Add(_textInstruction);
        SetSize(2, 1);
        SetPosition(x, y);

        OnValueSet = ValueSet;
    }

    private void ValueSet()
    {
        if (_textInstruction != null)
        {
            _textInstruction.Text = Value;
        }
    }
}
