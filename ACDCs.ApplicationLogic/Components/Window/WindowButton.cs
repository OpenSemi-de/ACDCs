namespace ACDCs.ApplicationLogic.Components.Window;

using Sharp.UI;
using UraniumUI.Icons.FontAwesome;

public class WindowButton : Button
{
    private readonly Window _window;
    private readonly WindowButtonType _windowButtonType;

    public WindowButton(WindowButtonType windowButtonType, Window window)
    {
        _windowButtonType = windowButtonType;
        _window = window;
        string text;
        switch (_windowButtonType)
        {
            case WindowButtonType.Minimize:
                text = Solid.WindowMinimize;
                break;

            case WindowButtonType.Maximize:
                text = Solid.WindowMaximize;
                break;

            case WindowButtonType.Restore:
                text = Solid.WindowRestore;
                break;

            case WindowButtonType.Close:
                text = Solid.x;
                break;

            default:
                text = "@";
                break;
        }
        Text = text;

        Clicked += WindowButton_Clicked;
    }

    private void WindowButton_Clicked(object? sender, EventArgs e)
    {
        switch (_windowButtonType)
        {
            case WindowButtonType.Minimize:
                _window.Minimize();
                break;

            case WindowButtonType.Maximize:
                _window.Maximize();
                break;

            case WindowButtonType.Restore:
                _window.Restore();
                break;

            case WindowButtonType.Close:
                _window.Close();
                break;

            default:
                break;
        }
    }
}
