namespace ACDCs.API.Core.Components.Components;

using ACDCs.API.Windowing.Components.Window;

// ReSharper disable once UnusedType.Global
public class ComponentsWindow : Window
{
    public ComponentsWindow(WindowContainer? container) : base(container, "Components", "menu_components.json", true, GetView)
    {
        Start();
    }

    private static View GetView(Window window)
    {
        ComponentsView componentsView = new(window);
        window.MenuParameters.Add("ComponentsView", componentsView);

        return componentsView;
    }
}
