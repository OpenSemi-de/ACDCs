namespace ACDCs.API.Core.Components.Sensors;

using Windowing.Components.Window;

public class SensorsDisplayWindow : Window
{
    public SensorsDisplayWindow(WindowContainer? windowContainer) : base(windowContainer, "Sensors", childViewFunction: GetView)
    {
        Start();
        Maximize();
    }

    private static View GetView(Window arg)
    {
        return new SensorsDisplayView();
    }
}
