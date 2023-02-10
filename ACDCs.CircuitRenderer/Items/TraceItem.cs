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

    public void AddPart(Coordinate from, Coordinate to)
    {
        ((TraceDrawable)DrawableComponent).AddPart(from, to);
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
}
