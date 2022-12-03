using System.Threading.Tasks;
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
        Padding = new(2, 2, 2, 2);
        Margin = new(2, 2, 2, 2);
        MinimumWidthRequest = 80;
        MinimumHeightRequest = 32;
        ItemHeight = MinimumHeightRequest + Margin.Top + Margin.Bottom;
    }

    public double ItemHeight { get; set; }

    public string MenuCommand { get; set; }

    public MenuFrame? MenuFrame { get; set; }

    private void MenuButton_Clicked(object? sender, System.EventArgs e)
    {
        App.Call(() =>
        {
            if (MenuFrame != null)
            {
                MenuFrame.SetPosition(this);
                MenuFrame.IsVisible = true;
            }
            return Task.CompletedTask;
        }).Wait();
    }
}