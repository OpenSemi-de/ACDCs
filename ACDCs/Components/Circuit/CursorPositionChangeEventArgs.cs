namespace ACDCs.Components.Circuit;

public class CursorPositionChangeEventArgs
{
    public Point CursorPosition { get; }

    public CursorPositionChangeEventArgs(Point cursorPosition)
    {
        CursorPosition = cursorPosition;
    }
}
