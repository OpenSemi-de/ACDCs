using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Newtonsoft.Json;

namespace ACDCs.CircuitRenderer.Drawables;

public sealed class ResistorDrawable : DrawableComponent
{
    public TextInstruction? TextInstruction { get; set; }

    [JsonConstructor]
    public ResistorDrawable() : base(typeof(ResistorDrawable), null)
    {
        Setup("", 1, 1);
    }

    public ResistorDrawable(IWorksheetItem parent, string value, float x, float y) : base(typeof(ResistorDrawable), parent)
    {
        Initialize(value);

        Setup(value, x, y);
    }

    private void Initialize(string value)
    {
        DrawablePins.Add(new PinDrawable(ParentItem, 0, 0.5f));
        DrawablePins.Add(new PinDrawable(ParentItem, 1f, 0.5f));
        DrawInstructions.Add(new LineInstruction(0f, 0.5f, 0.2f, 0.5f));
        DrawInstructions.Add(new BoxInstruction(0.2f, 0.35f, 0.6f, 0.3f));
        DrawInstructions.Add(new LineInstruction(0.8f, 0.5f, 1f, 0.5f));
        TextInstruction = new TextInstruction(value, 0f, 12f, 0.5f, 1.35f);
        DrawInstructions.Add(TextInstruction);
    }

    private void Setup(string value = "N/A", float x = 0, float y = 0)
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
