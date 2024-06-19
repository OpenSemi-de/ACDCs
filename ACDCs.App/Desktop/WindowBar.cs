using ACDCs.Interfaces;

namespace ACDCs.App.Desktop;

using Sharp.UI;
using System.Collections.Concurrent;

/// <summary>
/// The window bar class for switching between windows.
/// </summary>
/// <seealso cref="Sharp.UI.StackLayout" />
/// <seealso cref="ACDCs.Interfaces.IWindowBar" />
public class WindowBar : StackLayout, IWindowBar
{
    private readonly ConcurrentDictionary<IAppModule, AppButton> _buttons = new();
    private readonly IThemeService _themeService;
    private readonly IWindowService _windowService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowBar"/> class.
    /// </summary>
    /// <param name="windowService">The window service.</param>
    /// <param name="themeService">The theme service.</param>
    public WindowBar(IWindowService windowService, IThemeService themeService)
    {
        _windowService = windowService;
        _themeService = themeService;
        _windowService.OnWindowChanged += WindowService_OnWindowChanged;
        this.Orientation(StackOrientation.Horizontal);
    }

    /// <summary>
    /// Brings to front.
    /// </summary>
    /// <param name="moduleView">The module view.</param>
    public void BringToFront(IAppModule moduleView)
    {
        PopupModule(moduleView);
    }

    private void PopupModule(IAppModule module)
    {
        int maxZ = _buttons.Values.Max(b => b.CommandParameter != null ? ((IAppModule)b.CommandParameter).GetZIndex() : 0);
        module.SetZIndex(maxZ + 1);
    }

    private void WindowBar_Clicked(object? sender, EventArgs e)
    {
        if (sender is AppButton { CommandParameter: IAppModule module })
        {
            if (module.IsMinimized)
            {
                module.Show();
            }
            else
            {
                PopupModule(module);
            }
        }
    }

    private void WindowService_OnWindowChanged(object? sender, WindowChangedEventArgs e)
    {
        if (e.Modules.Contains(e.ChangedModule))
        {
            if (e.ChangedModule.HasTaskbarEntry)
            {
                // was added
                AppButton button = new AppButton(_themeService)
                    .Text(e.ChangedModule.Title)
                    .WidthRequest(160)
                    .VerticalOptions(LayoutOptions.Fill);

                button.Clicked += WindowBar_Clicked;
                button.CommandParameter = e.ChangedModule;

                _buttons.TryAdd(e.ChangedModule, button);
                Children.Add(button);
            }
        }
        else
        {
            // was removed
            if (_buttons.TryRemove(e.ChangedModule, out AppButton? button))
            {
                Children.Remove(button);
            }
        }
    }
}