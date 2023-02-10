using System.Collections.Generic;
using System.Linq;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;

namespace ACDCs.CircuitRenderer.Drawables;

public class TraceDrawable : DrawableComponent
{
    private readonly List<TraceDrawablePart> _traceParts = new();

    public TraceDrawable(IWorksheetItem parent) : base(typeof(TraceDrawable), parent)
    {
        SetSize(1, 1);
        SetPosition(0, 0);
    }

    public void AddPart(Coordinate from, Coordinate to, PinDrawable fromPin, PinDrawable toPin)
    {
        LineInstruction lineInstruction = new(from.X, from.Y, to.X, to.Y, 3);
        DrawInstructions.Add(
            lineInstruction
        );

        TraceDrawablePart tracePart = GetOrAddPart(fromPin, toPin);
        tracePart.Instructions.Add(lineInstruction);
    }

    public int CountSubnets(PinDrawable? pin)
    {
        return _traceParts.Count(part => part.FromPin == pin || part.ToPin == pin);
    }

    public PinDrawable? GetPinFrom(LineInstruction selectedLine)
    {
        TraceDrawablePart? tracePart = _traceParts.FirstOrDefault(part => part.Instructions.Contains(selectedLine));
        return tracePart?.FromPin;
    }

    public PinDrawable? GetPinTo(LineInstruction selectedLine)
    {
        TraceDrawablePart? tracePart = _traceParts.FirstOrDefault(part => part.Instructions.Contains(selectedLine));
        return tracePart?.ToPin;
    }

    public bool IsLineBetween(LineInstruction lineInstruction, PinDrawable fromPin, PinDrawable toPin)
    {
        return _traceParts.Any(part => part.Instructions.Contains(lineInstruction) &&
                                       ((part.FromPin == fromPin && part.ToPin == toPin) ||
                                        (part.FromPin == toPin && part.ToPin == fromPin)));
    }

    private TraceDrawablePart GetOrAddPart(PinDrawable fromPin, PinDrawable toPin)
    {
        TraceDrawablePart? tracePart = _traceParts.FirstOrDefault(
            part => (part.FromPin == fromPin && part.ToPin == toPin) ||
                    (part.FromPin == toPin && part.ToPin == fromPin)
            );

        if (tracePart != null)
        {
            return tracePart;
        }

        tracePart = new TraceDrawablePart(fromPin, toPin);
        _traceParts.Add(tracePart);

        return tracePart;
    }
}

public class TraceDrawablePart
{
    public PinDrawable FromPin { get; set; }

    public List<LineInstruction> Instructions { get; set; } = new();

    public PinDrawable ToPin { get; set; }

    public TraceDrawablePart(PinDrawable fromPin, PinDrawable toPin)
    {
        FromPin = fromPin;
        ToPin = toPin;
    }
}
