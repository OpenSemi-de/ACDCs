namespace ACDCs.ApplicationLogic.Components.Components;

using Window;

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
