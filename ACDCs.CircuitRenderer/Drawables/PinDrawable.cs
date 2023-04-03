using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Sheet;
using Newtonsoft.Json;

namespace ACDCs.CircuitRenderer.Drawables;

public class PinDrawable : DrawableComponent
{
    private readonly TextInstruction _textInstruction;
    private Worksheet? _worksheet;

    public string PinText
    {
        get => _textInstruction.Text;
        set => _textInstruction.Text = value;
    }

    [JsonIgnore]
    public new Worksheet? Worksheet
    {
        get => _worksheet;
        set
        {
            _worksheet = value;
            Setup(Position.X, Position.Y);
        }
    }

    [JsonConstructor]
    public PinDrawable(IWorksheetItem? parent = null, float x = 1, float y = 1, string pinText = "") : base(typeof(PinDrawable), parent)
    {
        DrawInstructions.Add(new CircleInstruction(0, 0, 1, 1));
        _textInstruction = new TextInstruction(pinText, 0, 12, 0.5f, 1.2f);
        DrawInstructions.Add(_textInstruction);
        PinText = pinText;
        Setup(x, y);
    }

    public PinDrawable(IWorksheetItem parent, PinDrawable pin) : base(typeof(PinDrawable), parent)
    {
        PinText = pin.PinText;
        Position = new Coordinate(Position);
        Size = new Coordinate(Size);
        Worksheet = pin.Worksheet;
        ParentItem = pin.ParentItem;
        Setup(1, 1);
    }

    private void Setup(float x, float y)
    {
        if (ParentItem != null)
            if (!ParentItem.Pins.Contains(this))
            {
                ParentItem.Pins.Add(this);
            }

        SetSize(1, 1);
        SetPosition(x, y);
    }
}
