namespace ACDCs.ApplicationLogic.Components.Menu;

// ReSharper disable once ClassNeverInstantiated.Global
public class MenuItemDefinition
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Action? ClickAction { get; set; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public string Handler { get; set; } = string.Empty;

    // ReSharper disable once MemberCanBeMadeStatic.Global
#pragma warning disable CA1822
    public string IsChecked => string.Empty;
#pragma warning restore CA1822

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public string MenuCommand { get; set; } = string.Empty;

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public List<MenuItemDefinition>? MenuItems { get; set; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public string Text { get; set; } = string.Empty;
}
