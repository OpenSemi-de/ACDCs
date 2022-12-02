using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace ACDCs.Views.Components.Menu;

public class MenuButton : Button, IMenuItem
{
    public MenuButton(string text, string menuCommand)
    {
        Text = text;
        MenuCommand = menuCommand;
        Clicked += MenuButton_Clicked;
        Padding = new Thickness(2, 2, 2, 2);
        Margin = new Thickness(2, 2, 2, 2);
        MinimumWidthRequest = 80;
        MinimumHeightRequest = 32;
        ItemHeight = MinimumHeightRequest + Margin.Top + Margin.Bottom;
    }

    private void MenuButton_Clicked(object? sender, System.EventArgs e)
    {
        MenuFrame.HideAllMenus();
        if (MenuFrame != null)
        {

           MenuFrame.SetPosition(this);
            MenuFrame.IsVisible = true;
        }
    }

    public string MenuCommand { get; set; }
    public double ItemHeight { get; set; }
    public MenuFrame? MenuFrame { get; set; }
}