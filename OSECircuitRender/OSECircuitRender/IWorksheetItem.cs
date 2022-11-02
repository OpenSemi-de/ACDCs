namespace OSECircuitRender
{
    public interface IWorksheetItem
    {
        IDrawableComponent DrawableComponent { get; set; }
        string RefName { get; internal set; }
    }
}