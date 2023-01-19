using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;

namespace ACDCs.CircuitRenderer.Drawables;

public sealed class TransistorDrawable : DrawableComponent
{
    private TextInstruction? _textInstruction;

    public TransistorDrawable(IWorksheetItem parent, TransistorDrawableType type, float x, float y) : base(
        typeof(TransistorDrawable), parent)
    {
        Setup(type, x, y);
    }

    private void Setup(TransistorDrawableType type = TransistorDrawableType.Pnp, float x = 0,
        float y = 0)
    {
        DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.5f, 0.5f));
        DrawInstructions.Add(new LineInstruction(0.5f, 0.5f, 1f, 0.2f));
        DrawInstructions.Add(new LineInstruction(1f, 0f, 1f, 0.2f));
        DrawInstructions.Add(new LineInstruction(1f, 0.8f, 1f, 1f));
        DrawInstructions.Add(new LineInstruction(0.5f, 0.5f, 1f, 0.8f));
        DrawInstructions.Add(new BoxInstruction(0.5f, 0.3f, 0.05f, 0.4f, new Color(0, 0, 0)));
        _textInstruction = new TextInstruction(Value, 0f, 12f, 0.3f, 1f);
        DrawInstructions.Add(_textInstruction);

        switch (type)
        {
            case TransistorDrawableType.Pnp:
                DrawablePins.Add(new PinDrawable(ParentItem, 0, 0.5f, "B"));
                DrawablePins.Add(new PinDrawable(ParentItem, 1f, 0f, "E"));
                DrawablePins.Add(new PinDrawable(ParentItem, 1f, 1f, "C"));

                DrawInstructions.Add(new LineInstruction(0.7f, 0.4f, 0.7f, 0.25f));
                DrawInstructions.Add(new LineInstruction(0.7f, 0.4f, 0.85f, 0.4f));
                DrawInstructions.Add(new LineInstruction(0.7f, 0.25f, 0.85f, 0.4f));
                break;

            case TransistorDrawableType.Npn:
                DrawablePins.Add(new PinDrawable(ParentItem, 0, 0.5f, "B"));
                DrawablePins.Add(new PinDrawable(ParentItem, 1f, 0f, "C"));
                DrawablePins.Add(new PinDrawable(ParentItem, 1f, 1f, "E"));

                DrawInstructions.Add(new LineInstruction(0.8f, 0.7f, 0.8f, 0.55f));
                DrawInstructions.Add(new LineInstruction(0.8f, 0.7f, 0.65f, 0.7f));
                DrawInstructions.Add(new LineInstruction(0.65f, 0.7f, 0.8f, 0.55f));
                break;
        }

        SetSize(2, 2);
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
