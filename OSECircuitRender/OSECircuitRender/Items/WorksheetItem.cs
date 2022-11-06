using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Items;

public class WorksheetItem : IWorksheetItem
{
    public IDrawableComponent DrawableComponent { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Rotation { get; set; }
    public string RefName { get; set; }
}