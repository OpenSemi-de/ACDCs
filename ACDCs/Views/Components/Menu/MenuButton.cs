using System.Threading.Tasks;
using ACDCs.Views.Components.Menu.MenuHandlers;
using Microsoft.Maui;
using Sharp.UI;


namespace ACDCs.Views.Components.Menu;

public class MenuButton : Button, IMenuItem
{
    public MenuButton(string text, string menuCommand)
    {
        MenuCommand = menuCommand;
        Clicked += MenuButton_Clicked;
        this
            .Text(text)
            .Padding((Thickness?)new(2, 2, 2, 2))
            .Margin((Thickness?)new(2, 2, 2, 2))
            .MinimumWidthRequest(80)
            .MinimumHeightRequest(32);

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

            if (MenuCommand != "")
            {
                MenuHandler.Call(MenuCommand);
            }

            return Task.CompletedTask;
        }).Wait();
    }
}
