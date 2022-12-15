using Microsoft.Maui.Graphics;

namespace ACDCs.Views.Components.CircuitView
{
    public class CursorPositionChangeEventArgs
    {
        public CursorPositionChangeEventArgs(Point cursorPosition)
        {
            CursorPosition = cursorPosition;
        }

        public Point CursorPosition { get; }
    }
}
