namespace ACDCs.Views.Components.Menu;

public class MenuItemDefinition
{
    public Action? ClickAction { get; set; }
    public string IsChecked { get; set; } = "";
    public string MenuCommand { get; set; } = string.Empty;
    public List<MenuItemDefinition>? MenuItems { get; set; }
    public string Text { get; set; } = string.Empty;
}
