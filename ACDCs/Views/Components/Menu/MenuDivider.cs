namespace ACDCs.Views.Components.Menu;

using Sharp.UI;

public class MenuDivider : Frame, IMenuItem
{
    public double ItemHeight { get; set; } = 4;
    public double ItemWidth { get; set; }

    public string MenuCommand { get; set; } = "";

    public List<IMenuItem> MenuItems { get; set; } = new();

    public string Text { get; set; } = "";

    public MenuDivider()
    {
        HeightRequest = 2;
        CornerRadius = 1;
        Margin = 1;
        Padding = 1;
    }
}
