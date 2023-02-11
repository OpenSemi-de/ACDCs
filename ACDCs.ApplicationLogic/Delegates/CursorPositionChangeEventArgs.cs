namespace ACDCs.ApplicationLogic.Delegates;

public class CursorPositionChangeEventArgs
{
    private Point _cursorPosition;

    public CursorPositionChangeEventArgs(Point cursorPosition)
    {
        _cursorPosition = cursorPosition;
    }
}
