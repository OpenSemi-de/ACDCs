using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Sheet;

namespace ACDCs.CircuitRenderer.Drawables;

public class PinDrawable : DrawableComponent
{
    public PinDrawable(IWorksheetItem parent, float x, float y, string pinText = "") : base(typeof(PinDrawable), parent)
    {
        PinText = pinText;
        DrawInstructions.Add(new CircleInstruction(0, 0, 1, 1));
        DrawInstructions.Add(new TextInstruction(pinText, 0, 12, 0.5f, 1.2f));
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

    public string PinText { get; }

    public new Worksheet? Worksheet
    {
        get => _worksheet;
        set
        {
            _worksheet = value;
            Setup(Position.X, Position.Y);
        }
    }

    private Worksheet? _worksheet;

    private void Setup(float x, float y)
    {
        if (ParentItem != null && !ParentItem.Pins.Contains(this))
        {
            ParentItem.Pins.Add(this);
        }

        SetSize(1, 1);
        SetPosition(x, y);
    }
}
