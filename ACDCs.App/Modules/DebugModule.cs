namespace ACDCs.App.Modules;

using ACDCs.App.Desktop;
using ACDCs.Interfaces;
using ACDCs.Interfaces.Modules;
using ACDCs.Interfaces.View;
using MauiIcons.Material;
using MetroLog.Maui;
using Microsoft.Extensions.Logging;
using Sharp.UI;

/// <summary>
/// The debug module class.
/// </summary>
/// <seealso cref="Interfaces.Modules.IStartMenuModule" />
/// <seealso cref="ACDCs.App.Desktop.ModuleView" />
public class DebugModule : ModuleView, IStartMenuModule
{
    private readonly ILogger _log;
    private readonly LogController _logController = new();
    private readonly Label _logs;
    private readonly ScrollView _scrollView;
    private readonly Timer _timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="DebugModule"/> class.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="themeService">The theme service.</param>
    /// <param name="desktopView">The desktop view.</param>
    public DebugModule(ILogger log, IThemeService themeService, IDesktopView desktopView) : base(themeService, desktopView)
    {
        _scrollView = new ScrollView()
            .HorizontalScrollBarVisibility(ScrollBarVisibility.Never)
            .VerticalScrollBarVisibility(ScrollBarVisibility.Always)
            .AbsoluteLayoutBounds(0, 0, 1, 1)
            .AbsoluteLayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);

        _logs = new Label()
            .HorizontalOptions(LayoutOptions.Fill);

        _scrollView.Content = _logs;

        Content.Add(_scrollView);
        _log = log;
        _log.LogInformation("Debug started.");

        _timer = new Timer(UpdateLogs);
        _timer.Change(1000, 1000);
    }

    /// <summary>
    /// Gets the start menu title.
    /// </summary>
    /// <value>
    /// The start menu title.
    /// </value>
    public static string StartMenuTitle { get; } = "Debug Window";

    /// <summary>
    /// Gets the icon.
    /// </summary>
    /// <value>
    /// The icon.
    /// </value>
    public override Enum Icon { get => MaterialIcons.DeveloperBoard; }

    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public override string Title { get => StartMenuTitle; }

    private async void DisplayLogs()
    {
        try
        {
            if (_logController.CanGetLogsString)
            {
                List<string>? list = await _logController.GetLogList();
                _logs.Text = string.Join(Environment.NewLine, list ?? []);
                await _scrollView.ScrollToAsync(0, _logs.Height, true);
            }
            else
            {
                _logs.Text = "You need to add a MemoryTarget to the configuration or use AddInMemoryLogger on the maui app builder to retrieved logs as a string...";
            }
        }
        catch (Exception value)
        {
            Console.WriteLine($"MetroLog.LogController|ERROR while displaying logs: {value}");
        }
    }

    private void UpdateLogs(object? state)
    {
        TaskHelper.Run(DisplayLogs);
    }
}