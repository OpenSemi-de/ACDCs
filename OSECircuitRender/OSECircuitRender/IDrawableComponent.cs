namespace OSECircuitRender
{
    public interface IDrawableComponent
    {
        DrawInstructions DrawInstructions { get; }
        DrawablePins DrawablePins { get; }
        DrawCoordinate Position { get; }
        DrawCoordinate Size { get; }
    }
}