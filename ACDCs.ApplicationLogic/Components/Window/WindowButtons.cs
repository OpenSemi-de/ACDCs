namespace ACDCs.ApplicationLogic.Components.Window;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

public class WindowButtons : Grid
{
    private readonly Window _window;
    private readonly WindowButton _minimizeButton;
    private readonly WindowButton _maximizeButton;
    private readonly WindowButton _restoreButton;
    private readonly WindowButton _closeButton;

    public WindowButtons(Window window)
    {
        _window = window;
        this.ColumnDefinitions(
            new ColumnDefinitionCollection
            {
                new ColumnDefinition(34),
                new ColumnDefinition(34),
                new ColumnDefinition(34),
            }
        );

        _minimizeButton = new WindowButton(WindowButtonType.Minimize, window);
        _maximizeButton = new WindowButton(WindowButtonType.Maximize, window);
        _restoreButton = new WindowButton(WindowButtonType.Restore, window);
        _closeButton = new WindowButton(WindowButtonType.Close, window);
        _restoreButton.IsVisible = false;
        this.SetRowAndColumn(_minimizeButton, 0, 0);
        this.SetRowAndColumn(_maximizeButton, 0, 1);
        this.SetRowAndColumn(_restoreButton, 0, 1);
        this.SetRowAndColumn(_closeButton, 0, 2);

        Add(_minimizeButton);
        Add(_maximizeButton);
        Add(_restoreButton);
        Add(_closeButton);
    }

    public void ShowRestore()
    {
        _restoreButton.IsVisible = true;
        _maximizeButton.IsVisible = false;
    }

    public void ShowMaximize()
    {
        _restoreButton.IsVisible = false;
        _maximizeButton.IsVisible = true;
    }
}
