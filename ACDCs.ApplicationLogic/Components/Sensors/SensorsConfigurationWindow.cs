namespace ACDCs.API.Core.Components.Sensors;

using Windowing.Components.Window;

public class SensorsConfigurationWindow : Window
{
    public SensorsConfigurationWindow(WindowContainer? container) : base(container, "Sensor configuration", null, false, GetView)
    {
        Start();
    }

    private static View GetView(Window arg)
    {
        return new SensorsConfigurationView();
    }
}
