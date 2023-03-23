using System.Linq;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Items;

namespace ACDCs.CircuitRenderer.Drawables;

public class TextDrawable : DrawableComponent
{
    private readonly TextInstruction _textInstruction;

    public bool IsRealFontSize
    {
        get => ((TextInstruction)DrawInstructions.First()).IsRealFontSize;
        set => ((TextInstruction)DrawInstructions.First()).IsRealFontSize = value;
    }

    public TextDrawable(WorksheetItem parent, string value, float textSize, float x, float y) : base(
            typeof(TextDrawable), parent)
    {
        _textInstruction = new TextInstruction(value, 0f, textSize, 0.5f, 0.5f);
        DrawInstructions.Add(_textInstruction);
        SetSize(2, 1);
        SetPosition(x, y);
        OnValueSet = ValueSet;
    }

    private void ValueSet()
    {
        _textInstruction.Text = Value;
    }
}
