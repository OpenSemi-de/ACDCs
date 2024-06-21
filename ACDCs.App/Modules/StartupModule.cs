namespace ACDCs.App.Modules;

using ACDCs.App.Desktop;
using ACDCs.Interfaces;
using ACDCs.Interfaces.Modules;
using ACDCs.Interfaces.View;
using Microsoft.Extensions.Logging;
using Sharp.UI;

/// <summary>
/// The splash screen module class.
/// </summary>
/// <seealso cref="ACDCs.App.Desktop.ModuleView" />
/// <seealso cref="Interfaces.Modules.IStartMenuModule" />
public class StartupModule : ModuleView, IAutoStartModule
{
    private readonly ILogger _logger;
    private readonly Label _splashLabel;
    private readonly Label _statusLabel;

    /// <summary>
    /// Initializes a new instance of the <see cref="StartupModule"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="themeService">The theme service.</param>
    /// <param name="desktopView">The desktop view.</param>
    public StartupModule(ILogger logger, IThemeService themeService, IDesktopView desktopView) : base(themeService, desktopView)
    {
        _logger = logger;

        _logger.LogInformation("Starting...");
        this.AbsoluteLayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.PositionProportional);
        this.AbsoluteLayoutBounds(0.5, 0.5, 300, 200);

        _splashLabel = new Label("ACDCs")
            .FontSize(40)
            .TextCenter()
            .AbsoluteLayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.PositionProportional)
            .AbsoluteLayoutBounds(0.5, 0.5, 300, 100);
        Add(_splashLabel);

        _statusLabel = new Label("Starting...")
            .FontSize(11)
            .TextCenter()
            .AbsoluteLayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.PositionProportional)
            .AbsoluteLayoutBounds(0.5, 0.8, 300, 20);
        Add(_statusLabel);

        Timer timer = new(CloseDialog);
        timer.Change(3000, Timeout.Infinite);

        _logger.LogInformation("Startup done");
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance has bottom bar.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has bottom bar; otherwise, <c>false</c>.
    /// </value>
    public override bool HasBottomBar { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this instance has icon.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has icon; otherwise, <c>false</c>.
    /// </value>
    public override bool HasIcon { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this instance has movement.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has movement; otherwise, <c>false</c>.
    /// </value>
    public override bool HasMovement { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this instance has resize element.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has resize element; otherwise, <c>false</c>.
    /// </value>
    public override bool HasResizeElement { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this instance has taskbar entry.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has taskbar entry; otherwise, <c>false</c>.
    /// </value>
    public override bool HasTaskbarEntry { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether this instance has title.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has title; otherwise, <c>false</c>.
    /// </value>
    public override bool HasTitle { get; set; } = false;

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public override string Title { get; set; } = "";

    private void CloseDialog(object? state)
    {
        base.Quit();
    }
}