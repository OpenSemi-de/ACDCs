namespace ACDCs.API.Windowing.Components.Window;

using Sharp.UI;

public class WindowStarterButton : Button
{
    private readonly WindowContainer _container;
    private readonly Type _startType;

    public WindowStarterButton(string text, Type startType, WindowContainer container) : base(text)
    {
        _container = container;
        _startType = startType;

        Clicked += WindowStarterButton_Clicked;
    }

    private void WindowStarterButton_Clicked(object? sender, EventArgs e)
    {
        Activator.CreateInstance(_startType, _container);
    }
}
