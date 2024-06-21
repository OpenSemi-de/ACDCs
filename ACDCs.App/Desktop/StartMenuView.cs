using ACDCs.Interfaces;

namespace ACDCs.App.Desktop;

using ACDCs.App;
using ACDCs.Interfaces.Modules;
using ACDCs.Interfaces.View;
using Microsoft.Maui.Layouts;
using Sharp.UI;
using System.Reflection;
using Rect = Microsoft.Maui.Graphics.Rect;

/// <summary>
/// StartmenuView class.
/// </summary>
/// <seealso cref="Sharp.UI.VerticalStackLayout" />
/// <seealso cref="Interfaces.View.IStartMenuView" />
public class StartMenuView : AppBorderedVerticalStackLayout, IStartMenuView
{
    private readonly IThemeService _themeService;
    private readonly IWindowService _windowService;

    /// <summary>
    /// Initializes a new instance of the <see cref="StartMenuView" /> class.
    /// </summary>
    /// <param name="themeService">The color service.</param>
    /// <param name="windowService">The window service.</param>
    public StartMenuView(IThemeService themeService, IWindowService windowService) : base(themeService)
    {
        Shadow = new Shadow
        {
            Offset = new Microsoft.Maui.Graphics.Point(4, 4),
            Brush = new SolidColorBrush(themeService.GetColor(ColorDefinition.Shadow))
        };

        _themeService = themeService;
        _themeService.ThemeChanged += ThemeService_ThemeChanged;
        _windowService = windowService;

        this.AbsoluteLayoutFlags(AbsoluteLayoutFlags.YProportional)
            .AbsoluteLayoutBounds(new Rect(0, 1, 240, 500))
            .BackgroundColor(themeService.GetColor(ColorDefinition.StartMenuBackground));

        ZIndex = int.MaxValue - 1;
        IsVisible = false;
    }

    /// <summary>
    /// Hides this instance.
    /// </summary>
    public void Hide()
    {
        TaskHelper.Run(() =>
        {
            IsVisible = false;
        });
    }

    /// <summary>
    /// Shows this instance.
    /// </summary>
    public void Show()
    {
        TaskHelper.Run(() =>
        {
            IsVisible = true;
        });
    }

    /// <summary>
    /// Starts this instance.
    /// </summary>
    public void Start()
    {
        FillStartMenu();
        RunAutostart();
    }

    /// <summary>
    /// Creates the button.
    /// </summary>
    /// <param name="ModuleView">The module view.</param>
    /// <returns></returns>
    private AppButton CreateButton(TypeInfo? ModuleView)
    {
        string startMenuTitle = ModuleView?.GetProperty("StartMenuTitle")?.GetValue(ModuleView) as string ?? "";

        AppButton item = new AppButton(_themeService)
            .Text(startMenuTitle)
            .HorizontalOptions(LayoutOptions.Fill)
            .HeightRequest(40);

        item.CommandParameter = ModuleView;
        item.Clicked += Item_Clicked;
        return item;
    }

    /// <summary>
    /// Fills the start menu.
    /// </summary>
    private void FillStartMenu()
    {
        List<TypeInfo> ModuleViews = _windowService.GetModuleViews();

        foreach (TypeInfo? ModuleView in ModuleViews)
        {
            AppButton item = CreateButton(ModuleView);
            Children.Add(item);
        }
    }

    /// <summary>
    /// Raised when a startmenu item is clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void Item_Clicked(object? sender, EventArgs e)
    {
        if (sender is AppButton button &&
            button.CommandParameter is TypeInfo typeInfo)
        {
            if (typeInfo.ImplementedInterfaces.Contains(typeof(ISingleInstanceModule)))
            {
                _windowService.Popup(typeInfo);
            }
            else
            {
                _windowService.Start(typeInfo);
            }

            Task.Run(async () =>
            {
                await Task.Delay(300);
                Hide();
            });
        };
    }

    /// <summary>
    /// Runs the autostart.
    /// </summary>
    private void RunAutostart()
    {
        List<TypeInfo> AutostartModuleViews = _windowService.GetAutoStartViews();

        foreach (TypeInfo? AutostartModuleView in AutostartModuleViews)
        {
            _windowService.Start(AutostartModuleView);
        }
    }

    /// <summary>
    /// The theme changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ThemeService_ThemeChanged(object? sender, EventArgs e)
    {
        this.BackgroundColor(_themeService.GetColor(ColorDefinition.StartMenuBackground));
    }
}