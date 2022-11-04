namespace OSECircuitRender.Interfaces
{
    public interface IWorksheetItem
    {
        IDrawableComponent DrawableComponent { get; set; }
        string RefName { get; internal set; }
    }
}