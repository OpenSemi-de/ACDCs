namespace ACDCs.App.GUI.Modules;

using ACDCs.App.Desktop;
using ACDCs.Interfaces;
using ACDCs.Interfaces.Modules;
using ACDCs.Interfaces.View;
using ACDCs.Shared;
using MauiIcons.Material;
using Microsoft.Extensions.Logging;

/// <summary>
/// The module for the circuit editor.
/// </summary>
/// <seealso cref="ACDCs.App.Desktop.ModuleView" />
/// <seealso cref="Interfaces.Modules.IStartMenuModule" />
public class EditorModule : ModuleView, IStartMenuModule
{
    private readonly ICircuitComponentView _components;
    private readonly ICircuitControllerView _controller;
    private readonly IDesktopView _desktopView;
    private readonly ICircuitEditorView _editor;
    private readonly ILogger _log;
    private readonly IThemeService _themeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditorModule" /> class.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="themeService">The theme service.</param>
    /// <param name="desktopView">The desktop view.</param>
    public EditorModule(ILogger log, IThemeService themeService, IDesktopView desktopView) : base(themeService, desktopView)
    {
        _log = log;
        _themeService = themeService;
        _desktopView = desktopView;

        _editor = ServiceHelper.GetService<ICircuitEditorView>();
        _controller = ServiceHelper.GetService<ICircuitControllerView>();
        _controller.RenderCore = _editor.RenderCore;
        _components = ServiceHelper.GetService<ICircuitComponentView>();
        _components.RenderCore = _editor.RenderCore;

        Content.Add((IView)_editor);
        Content.Add((IView)_controller);
        Content.Add((IView)_components);

        _log.LogDebug("Editor started.");
    }

    /// <summary>
    /// Gets the start menu title.
    /// </summary>
    /// <value>
    /// The start menu title.
    /// </value>
    public static string StartMenuTitle { get; } = "New Editor";

    /// <summary>
    /// Gets the icon.
    /// </summary>
    /// <value>
    /// The icon.
    /// </value>
    public override Enum Icon { get => MaterialIcons.EditDocument; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is maximized.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is maximized; otherwise, <c>false</c>.
    /// </value>
    public override bool IsMaximized { get; set; } = true;

    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public override string Title { get => "Editor: New file"; }
}