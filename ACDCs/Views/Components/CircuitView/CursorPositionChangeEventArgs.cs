namespace ACDCs.Views.Components.CircuitView;

public class CursorPositionChangeEventArgs
{
    public Point CursorPosition { get; }

    public CursorPositionChangeEventArgs(Point cursorPosition)
    {
        CursorPosition = cursorPosition;
    }
}
