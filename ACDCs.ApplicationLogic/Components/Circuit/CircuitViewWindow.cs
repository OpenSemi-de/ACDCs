using ACDCs.ApplicationLogic.Components.Window;

namespace ACDCs.ApplicationLogic.Components.Circuit;

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
