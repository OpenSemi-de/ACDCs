namespace ACDCs.ApplicationLogic.Components.Window;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

public class WindowStarterButton : Button
{
    private readonly WindowContainer? _container;
    private readonly Type _startType;

    public WindowStarterButton(string text, Type? startType = null, WindowContainer? container = null) : base(text)
    {
        if (startType == null || container == null)
        {
            return;
        }

        _container = container;
        _startType = startType;

        Clicked += WindowStarterButton_Clicked;
    }

    private void WindowStarterButton_Clicked(object? sender, EventArgs e)
    {
        var startedInstance = Activator.CreateInstance(_startType, _container);
    }
}
