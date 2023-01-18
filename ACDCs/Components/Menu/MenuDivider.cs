using ACDCs.Interfaces;

namespace ACDCs.Components.Menu;

using Sharp.UI;

public class MenuDivider : Frame, IMenuComponent
{
    public double ItemHeight { get; set; } = 4;
    public double ItemWidth { get; set; }
    public string MenuCommand { get; set; } = string.Empty;

    // ReSharper disable once UnusedMember.Global
    public List<IMenuComponent> MenuItems { get; set; } = new();

    public string Text { get; set; } = string.Empty;

    public MenuDivider()
    {
        HeightRequest = 2;
        CornerRadius = 1;
        Margin = 1;
        Padding = 1;
    }
}
