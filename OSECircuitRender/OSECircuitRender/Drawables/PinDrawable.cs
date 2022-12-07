using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;
using System.Linq;
using Newtonsoft.Json;
using OSECircuitRender.Sheet;

namespace OSECircuitRender.Drawables;

public class PinDrawable : DrawableComponent
{
    private Worksheet? _worksheet;

    [JsonIgnore]
    public new IWorksheetItem? BackRef => Worksheet?.Items.FirstOrDefault(item => item.Pins.Contains(this));

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
        //BackRef = pin.BackRef;
        Setup(1, 1);
    }

    public string PinText { get; }

    private void Setup(float x, float y)
    {
        if (ParentItem != null && !ParentItem.Pins.Contains(this))
        {
            ParentItem.Pins.Add(this);
        }
        SetSize(1, 1);
        SetPosition(x, y);

    }

    public new Worksheet? Worksheet
    {
        get => _worksheet;
        set
        {
            _worksheet = value;
            Setup(Position.X, Position.Y);
        }
    }
}