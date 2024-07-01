namespace ACDCs.App.GUI.Modules;

using ACDCs.App.Desktop;
using ACDCs.Interfaces;
using ACDCs.Interfaces.Modules;
using ACDCs.Interfaces.View;
using ACDCs.Structs;
using MauiIcons.Material;
using Microsoft.Extensions.Logging;
using Sharp.UI;

/// <summary>
/// The file module class.
/// </summary>
/// <seealso cref="ACDCs.App.Desktop.ModuleView" />
/// <seealso cref="Interfaces.Modules.IStartMenuModule" />
/// <seealso cref="Interfaces.Modules.IAutoStartModule" />
/// <seealso cref="Interfaces.Modules.ISingleInstanceModule" />
/// <seealso cref="IStartMenuModule" />
/// <seealso cref="ModuleView" />
public class FileModule : ModuleView, IStartMenuModule, IAutoStartModule, ISingleInstanceModule
{
    private readonly HorizontalStackLayout _buttonGrid;
    private readonly Dictionary<AppButton, AppBorderedVerticalStackLayout> _buttons;
    private readonly IDesktopView _desktopView;
    private readonly ILogger _log;
    private readonly IThemeService _themeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileModule" /> class.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="themeService">The theme service.</param>
    /// <param name="desktopView">The desktop view.</param>
    public FileModule(ILogger log, IThemeService themeService, IDesktopView desktopView) : base(themeService, desktopView)
    {
        _desktopView = desktopView;
        _log = log;
        _themeService = themeService;
        this.AbsoluteLayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.WidthProportional)
            .AbsoluteLayoutBounds(0, 0, 1, 40);

        _buttonGrid = [];
        _buttonGrid
            .AbsoluteLayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All)
            .AbsoluteLayoutBounds(0, 0, 1, 1);

        _buttons = new Dictionary<AppButton, AppBorderedVerticalStackLayout>
        {
            { CreateButton("File"), CreateMenu("File") }
        };

        AttachButtonToMenus();
        _desktopView.OnDesktopClick += DesktopView_OnDesktopClick;
        Content.Add(_buttonGrid);
    }

    /// <summary>
    /// Gets the start menu title.
    /// </summary>
    /// <value>
    /// The start menu title.
    /// </value>
    public static string StartMenuTitle { get; } = "File commands";

    /// <summary>
    /// Gets or sets a value indicating whether this instance has bottom bar.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance has bottom bar; otherwise, <c>false</c>.
    /// </value>
    public override bool HasBottomBar { get => false; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance has movement.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance has movement; otherwise, <c>false</c>.
    /// </value>
    public override bool HasMovement { get => false; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance has resize element.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance has resize element; otherwise, <c>false</c>.
    /// </value>
    public override bool HasResizeElement { get => false; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance has title.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance has title; otherwise, <c>false</c>.
    /// </value>
    public override bool HasTitle { get => false; }

    /// <summary>
    /// Gets or sets the icon.
    /// </summary>
    /// <value>
    /// The icon.
    /// </value>
    public override Enum Icon { get => MaterialIcons.AttachFile; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public override string Title { get => StartMenuTitle; }

    private void AttachButtonToMenus()
    {
        foreach (var kv in _buttons)
        {
            var button = kv.Key;
            var menu = kv.Value;
            button.Clicked += Menu_Button_Clicked;
            button.CommandParameter = menu;
        }
    }

    private AppButton CreateButton(string text)
    {
        AppButton button = new AppButton(_themeService)
            .Text(text)
            .WidthRequest(100);
        _buttonGrid.Add(button);
        return button;
    }

    private AppBorderedVerticalStackLayout CreateMenu(string menuName)
    {
        AppBorderedVerticalStackLayout layout = new AppBorderedVerticalStackLayout(_themeService)
            .ZIndex(10)
            .WidthRequest(140)
            .AbsoluteLayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.None)
            .AbsoluteLayoutBounds(0, 40, 140, 200);

        layout.BackgroundColor = _themeService.GetColor(ColorDefinition.ModuleBackground);
        layout.Stroke = new SolidColorBrush(_themeService.GetColor(ColorDefinition.Border));
        layout.StrokeThickness = 2;

        layout.Shadow = new Shadow
        {
            Offset = new Microsoft.Maui.Graphics.Point(4, 4)
        };

        layout.IsVisible = false;
        layout.Shadow.Brush = new SolidColorBrush(_themeService.GetColor(ColorDefinition.Shadow));
        _desktopView.AddComponent(layout);
        return layout;
    }

    private void DesktopView_OnDesktopClick(object? sender, EventArgs e)
    {
        foreach (var kv in _buttons)
        {
            var menu = kv.Value;
            menu.IsVisible = false;
        }
    }

    private void Menu_Button_Clicked(object? sender, EventArgs e)
    {
        if (sender is AppButton button && button.CommandParameter is AppBorderedVerticalStackLayout menu)
        {
            menu.ZIndex = ZIndex + 1;
            menu.IsVisible = true;
        }
    }
}