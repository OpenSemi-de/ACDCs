using ACDCs.Interfaces;

namespace ACDCs.App.Desktop;

using ACDCs.App;
using ACDCs.Interfaces.Modules;
using ACDCs.Interfaces.View;
using Sharp.UI;
using Rect = Microsoft.Maui.Graphics.Rect;

/// <summary>
/// TaskbarView class.
/// </summary>
/// <seealso cref="Interfaces.View.ITaskbarView" />
public class TaskbarView(IThemeService themeService, IStartButtonView startButtonView, IStartMenuView startMenuView, IWindowBarView windowBar) : ITaskbarView
{
    private readonly IStartButtonView _startButton = startButtonView;
    private readonly IStartMenuView _startMenu = startMenuView;
    private readonly IThemeService _themeService = themeService;
    private readonly IWindowBarView _windowBar = windowBar;
    private AbsoluteLayout? _layout;
    private AppBorderedHorizontalStackLayout? _taskbarLayout;

    /// <summary>
    /// Brings to front.
    /// </summary>
    /// <param name="moduleView">The module view.</param>
    public void BringToFront(IAppModule moduleView)
    {
        _windowBar.BringToFront(moduleView);
    }

    /// <summary>
    /// Starts the TaskbarView into the specified layout.
    /// </summary>
    /// <param name="layout">The layout.</param>
    /// <returns></returns>
    public async Task Start(AbsoluteLayout layout)
    {
        _layout = layout;
        _taskbarLayout = new(_themeService);
        _taskbarLayout.Children.Add((IView)_startButton);
        _taskbarLayout.Children.Add((IView)_windowBar);
        _taskbarLayout
            .BackgroundColor(_themeService.GetColor(ColorDefinition.StartMenuBackground))
            .AbsoluteLayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.WidthProportional | Microsoft.Maui.Layouts.AbsoluteLayoutFlags.YProportional)
            .AbsoluteLayoutBounds(new Rect(0, 1, 1, 40));

        _taskbarLayout.ZIndex = int.MaxValue;

        _layout.Children.Add((IView)_startMenu);
        _layout.Children.Add(_taskbarLayout);
        await _startButton.SetStartMenu(_startMenu);
        _startMenu.Start();
        _themeService.ThemeChanged += ThemeService_ThemeChanged;

        await Task.CompletedTask;
    }

    /// <summary>
    /// Handles the ThemeChanged event of the themeService control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    private void ThemeService_ThemeChanged(object? sender, EventArgs e)
    {
        _taskbarLayout?.BackgroundColor(_themeService.GetColor(ColorDefinition.StartMenuBackground));
    }
}