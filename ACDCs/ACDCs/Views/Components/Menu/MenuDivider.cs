using System.Collections.Generic;
using Microsoft.Maui.Controls;

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
    public string Text { get; set; } = "";
    public List<IMenuItem> MenuItems { get; set; } = new();
    public string MenuCommand { get; set; } = "";
    public double ItemHeight { get; set; } = 4;
}