using System.Collections.Generic;
using Sharp.UI;

namespace ACDCs.Views.Components.Menu;

public class MenuDivider : Frame, IMenuItem
{
    public MenuDivider()
    {
        HeightRequest = 2;
        CornerRadius = 1;
        Margin = 1;
        Padding = 1;
    }

    public double ItemHeight { get; set; } = 4;
    public string MenuCommand { get; set; } = "";
    public List<IMenuItem> MenuItems { get; set; } = new();
    public string Text { get; set; } = "";
}
