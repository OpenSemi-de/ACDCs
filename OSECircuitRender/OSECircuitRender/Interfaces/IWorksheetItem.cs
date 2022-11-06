namespace OSECircuitRender.Interfaces
{
    public interface IWorksheetItem
    {
        IDrawableComponent DrawableComponent { get; set; }

        float X { get; set; }
        float Y { get; set; }

        float Rotation { get; set; }

        string RefName { get; set; }
    }
}