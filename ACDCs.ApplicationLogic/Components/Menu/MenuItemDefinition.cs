namespace ACDCs.ApplicationLogic.Components.Menu;

public class MenuItemDefinition
{
    public Action? ClickAction { get; set; }
    public string Handler { get; set; } = string.Empty;
    public string IsChecked => string.Empty;
    public string MenuCommand { get; set; } = string.Empty;
    public List<MenuItemDefinition>? MenuItems { get; set; }
    public string Text { get; set; } = string.Empty;
}
