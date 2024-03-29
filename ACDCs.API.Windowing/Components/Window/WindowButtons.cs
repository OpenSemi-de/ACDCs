﻿namespace ACDCs.API.Windowing.Components.Window;

using API.Components;
using Sharp.UI;

public class WindowButtons : Grid
{
    private readonly WindowButton _closeButton;
    private readonly WindowButton _maximizeButton;
    private readonly WindowButton _minimizeButton;
    private readonly WindowButton _restoreButton;
    private bool _showOnlyClose;

    public WindowButtons(Window window)
    {
        this.ColumnDefinitions(
            new ColumnDefinitionCollection
            {
                new ColumnDefinition(34),
                new ColumnDefinition(34),
                new ColumnDefinition(34),
            }
        );
        this.Margin(new Thickness(0, 1, 1, 0));

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

    public void ShowMaximize()
    {
        if (_showOnlyClose) return;
        _restoreButton.IsVisible = false;
        _maximizeButton.IsVisible = true;
    }

    public void ShowOnlyClose()
    {
        _maximizeButton.IsVisible = false;
        _minimizeButton.IsVisible = false;
        _restoreButton.IsVisible = false;
        _showOnlyClose = true;
    }

    public void ShowRestore()
    {
        if (_showOnlyClose) return;
        _restoreButton.IsVisible = true;
        _maximizeButton.IsVisible = false;
    }
}
