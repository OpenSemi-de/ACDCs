namespace ACDCs.API.Core.Components.Sensors;

using Windowing.Components.Window;

public class SensorsConfigurationWindow : Window
{
    protected SensorsConfigurationWindow(WindowContainer? container) : base(container, "Sensor configuration", null, false, GetView)
    {
    }

    private static View GetView(Window arg)
    {
        return new SensorsConfigurationView();
    }
}
