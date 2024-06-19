using System.Linq;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Sheet;
using Newtonsoft.Json;

namespace ACDCs.CircuitRenderer.Drawables;

public class PinDrawable : DrawableComponent
{
    private Worksheet? _worksheet;

    public string PinText
    {
        get => TextInstruction?.Text;
        set
        {
            if (TextInstruction != null)
            {
                TextInstruction.Text = value;
            }
        }
    }

    public TextInstruction? TextInstruction { get; }

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
    public PinDrawable(IWorksheetItem? item = null) : base(
        typeof(PinDrawable), item)
    {
        Setup(1, 1);
    }

    public PinDrawable(IWorksheetItem? parent = null, float x = 1, float y = 1, string pinText = "") : base(typeof(PinDrawable), parent)
    {
        DrawInstructions.Add(new CircleInstruction(0, 0, 1, 1));
        TextInstruction = new TextInstruction(pinText, 0, 12, 0.5f, 1.2f);
        DrawInstructions.Add(TextInstruction);
        PinText = pinText;
        ParentItem = parent;
        Setup(x, y);
    }

    // public PinDrawable(IWorksheetItem parent, PinDrawable pin) : base(typeof(PinDrawable), parent)
    // {
    //     PinText = pin.PinText;
    //     Position = new Coordinate(Position);
    //     Size = new Coordinate(Size);
    //     Worksheet = pin.Worksheet;
    //     ParentItem = pin.ParentItem;
    //     Setup(1, 1);
    // }

    private void Setup(float x, float y)
    {
        if (ParentItem != null)
            if (!ParentItem.Pins.Contains(this) &&
                !ParentItem.Pins.Any(pin => pin.ComponentGuid.Equals(this.ComponentGuid)))
            {
                ParentItem.Pins.Add(this);
            }

        SetSize(1, 1);
        SetPosition(x, y);
    }
}
