using ACDCs.Views.Components.Menu.MenuHandlers;

namespace ACDCs.Views.Components.Menu;

using Sharp.UI;

public class MenuButton : Button, IMenuItem
{
    private readonly Action? _clickAction;
    public double ItemHeight { get; set; }

    public double ItemWidth { get; set; }
    public string MenuCommand { get; set; }
    public MenuFrame? MenuFrame { get; set; }

    public MenuButton(string text, string menuCommand, Action? clickAction = null)
    {
        _clickAction = clickAction;
        Text = text;
        MenuCommand = menuCommand;
        Clicked += clickAction != null ? ClickAction_Click : MenuButton_Clicked;
        Padding = new(2, 2, 2, 2);
        Margin = new(2, 2, 2, 2);
        WidthRequest = 100;
        MinimumHeightRequest = 32;
        ItemHeight = MinimumHeightRequest + Margin.Top + Margin.Bottom;
    }

    private void ClickAction_Click(object? sender, EventArgs e)
    {
        App.Call(() =>
        {
            _clickAction?.Invoke();
            if (MenuCommand != "")
            {
                MenuHandler.Call(MenuCommand);
            }

            return Task.CompletedTask;
        }).Wait();
    }

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
