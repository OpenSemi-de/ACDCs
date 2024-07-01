using ACDCs.Interfaces;

namespace ACDCs.App.Desktop;

using ACDCs.App;
using ACDCs.Interfaces.View;
using ACDCs.Structs;
using Sharp.UI;

/// <summary>
/// The Start Button class.
/// </summary>
/// <seealso cref="ACDCs.App.AppButton" />
/// <seealso cref="Interfaces.View.IStartButtonView" />
public class StartButtonView : AppButton, IStartButtonView
{
    private readonly IThemeService _themeService;
    private IStartMenuView? _startMenu;

    /// <summary>
    /// Initializes a new instance of the <see cref="StartButtonView"/> class.
    /// </summary>
    /// <param name="themeService">The color service.</param>
    public StartButtonView(IThemeService themeService) : base(themeService)
    {
        Text("Start")
            .WidthRequest(80)
            .HeightRequest(40);

        _themeService = themeService;

        Clicked += StartButtonView_Clicked;
    }

    /// <summary>
    /// Sets the start menu.
    /// </summary>
    /// <param name="startMenu">The start menu.</param>
    public async Task SetStartMenu(IStartMenuView startMenu)
    {
        _startMenu = startMenu;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Click event for start button.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private async void StartButtonView_Clicked(object? sender, EventArgs e)
    {
        bool isVisible = _startMenu != null && _startMenu.IsVisible;

        if (isVisible)
        {
            this.BackgroundColor(_themeService.GetColor(ColorDefinition.StartButtonReleased));
            _startMenu?.Hide();
        }
        else
        {
            this.BackgroundColor(_themeService.GetColor(ColorDefinition.StartButtonPressed));
            _startMenu?.Show();
        }

        await Task.CompletedTask;
    }
}