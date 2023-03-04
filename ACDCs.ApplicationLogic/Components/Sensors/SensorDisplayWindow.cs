namespace ACDCs.API.Core.Components.Sensors;

using Windowing.Components.Window;

public class SensorDisplayWindow : Window
{
    protected SensorDisplayWindow(WindowContainer? windowContainer) : base(windowContainer, "Sensors", childViewFunction: GetView)
    {
    }

    private static View GetView(Window arg)
    {
        return new SensorDisplayView();
    }
}
