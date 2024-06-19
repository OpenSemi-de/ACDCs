using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Newtonsoft.Json;

namespace ACDCs.CircuitRenderer.Drawables;

public sealed class DiodeDrawable : DrawableComponent
{
    private TextInstruction? _textInstruction;

    [JsonConstructor]
    public DiodeDrawable() : base(typeof(DiodeDrawable), null)
    {
        Setup("", 1, 1);
    }

    public DiodeDrawable(IWorksheetItem parent, string value, float x, float y) : base(typeof(DiodeDrawable), parent)
    {
        Initialize(value);
        Setup(value, x, y);
    }

    private void Initialize(string value)
    {
        DrawablePins.Add(new PinDrawable(ParentItem, 0, 0.5f));
        DrawablePins.Add(new PinDrawable(ParentItem, 1f, 0.5f));
        DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.5f, 0.5f));
        DrawInstructions.Add(new LineInstruction(0.8f, 0.5f, 0.5f, 0.2f));
        DrawInstructions.Add(new LineInstruction(0.8f, 0.5f, 0.5f, 0.8f));
        DrawInstructions.Add(new LineInstruction(0.5f, 0.2f, 0.5f, 0.8f));
        DrawInstructions.Add(new LineInstruction(0.8f, 0.5f, 1f, 0.5f));
        _textInstruction = new TextInstruction(value, 0f, 12f, 0.5f, 1.3f);
        DrawInstructions.Add(_textInstruction);
    }

    private void Setup(string value = "N/A", float x = 0, float y = 0)
    {
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
