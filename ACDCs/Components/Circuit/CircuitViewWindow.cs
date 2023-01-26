using ACDCs.Components.Window;

namespace ACDCs.Components.Circuit;

public class CircuitViewWindow : WindowView
{
    private static int s_circuitCount;

    private static int CircuitCount
    {
        get
        {
            s_circuitCount++;
            return s_circuitCount;
        }
    }

    public CircuitViewWindow() : base(API.MainContainer, $"Circuit {CircuitCount}")
    {
        Initiliaze();
    }

    private void Initiliaze() => WindowContent = new CircuitSheetView();
}
