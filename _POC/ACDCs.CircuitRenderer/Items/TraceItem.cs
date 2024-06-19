using System.Collections.Generic;
using System.Linq;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Instructions;

namespace ACDCs.CircuitRenderer.Items;

public class TraceItem : WorksheetItem
{
    private Dictionary<LineInstruction, Color> _colors = new();
    public override string DefaultValue => "";
    public override bool IsInsertable => false;
    public NetItem Net { get; set; }

    public TraceItem()
    {
        DrawableComponent = new TraceDrawable(this);
    }

    public void AddPart(Coordinate from, Coordinate to, PinDrawable fromPin, PinDrawable toPin)
    {
        ((TraceDrawable)DrawableComponent).AddPart(from, to, fromPin, toPin);
    }

    public void ResetColor()
    {
        foreach (LineInstruction line in DrawableComponent.DrawInstructions.OfType<LineInstruction>())
        {
            if (_colors.ContainsKey(line))
            {
                line.StrokeColor = _colors[line];
            }
        }
    }

    public void SetColor(Color color)
    {
        _colors.Clear();
        foreach (LineInstruction line in DrawableComponent.DrawInstructions.OfType<LineInstruction>())
        {
            _colors.Add(line, line.StrokeColor);
            line.StrokeColor = color;
        }
    }

    public void SetColorFromToPin(Color color, LineInstruction selectedLine)
    {
        _colors.Clear();

        if (DrawableComponent is not TraceDrawable traceDrawable)
        {
            return;
        }

        PinDrawable? fromPin = traceDrawable.GetPinFrom(selectedLine);
        PinDrawable? toPin = traceDrawable.GetPinTo(selectedLine);
        if (fromPin == null || toPin == null)
        {
            return;
        }

        foreach (LineInstruction line in traceDrawable.DrawInstructions.OfType<LineInstruction>())
        {
            _colors.Add(line, line.StrokeColor);

            if (traceDrawable.IsLineBetween(line, fromPin, toPin))
                line.StrokeColor = color;
        }
    }
}
