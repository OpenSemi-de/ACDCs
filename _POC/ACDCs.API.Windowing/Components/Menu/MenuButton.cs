namespace ACDCs.API.Windowing.Components.Menu;

using Instance;
using Interfaces;
using Sharp.UI;

public class MenuButton : Button, IMenuComponent
{
    private readonly Action? _clickAction;
    public double ItemHeight { get; set; } = 36;
    public double ItemWidth { get; set; } = 134;
    public string MenuCommand { get; set; }
    public MenuFrame? MenuFrame { get; set; }

    public MenuButton(string text, string menuCommand, Action? clickAction = null, string fontFamily = "")
    {
        _clickAction = clickAction;
        BackgroundColor = API.Instance.Foreground;
        TextColor = API.Instance.Text;
        BorderColor = Colors.Transparent;

        if (fontFamily != "")
        {
            FontFamily = fontFamily;
        }

        Text = text;
        MenuCommand = menuCommand;
        Clicked += clickAction != null ? ClickAction_Click : MenuButton_Clicked;
        Padding = 0;
        Margin = 1;
        CornerRadius = 1;
        WidthRequest = 130;
        MinimumHeightRequest = 32;
        HorizontalOptions = LayoutOptions.Fill;
    }

    private void ClickAction_Click(object? sender, EventArgs e)
    {
        API.Call(() =>
        {
            _clickAction?.Invoke();
            if (MenuCommand != "")
            {
                API.Instance.Call(MenuCommand);
            }

            return Task.CompletedTask;
        }).Wait();
    }

    private void MenuButton_Clicked(object? sender, EventArgs e)
    {
        API.Call(() =>
        {
            if (MenuFrame != null)
            {
                MenuFrame.SetPosition(this);
                MenuFrame.IsVisible = true;
            }

            if (MenuCommand != "")
            {
                API.Instance.Call(MenuCommand);
            }

            return Task.CompletedTask;
        }).Wait();
    }
}
