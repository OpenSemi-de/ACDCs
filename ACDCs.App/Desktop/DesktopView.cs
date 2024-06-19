namespace ACDCs.App.Desktop;

using ACDCs.Interfaces;
using Sharp.UI;
using System;
using System.Threading.Tasks;

/// <summary>
/// The DesktopView class.
/// </summary>
/// <seealso cref="Sharp.UI.ContentView" />
/// <seealso cref="ACDCs.Interfaces.IDesktopView" />
public class DesktopView : ContentView, IDesktopView
{
    private readonly TapGestureRecognizer _desktopClickRecognizer;
    private readonly AbsoluteLayout _layout;
    private readonly ITaskbarView _taskbar;
    private readonly IWindowService _windowService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DesktopView" /> class.
    /// </summary>
    /// <param name="taskbar">The taskbar.</param>
    /// <param name="windowService">The window service.</param>
    public DesktopView(ITaskbarView taskbar, IWindowService windowService)
    {
        _layout =
            new AbsoluteLayout()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _taskbar = taskbar;
        _windowService = windowService;
        _windowService.SetDesktop(this);

        _desktopClickRecognizer = new TapGestureRecognizer();
        GestureRecognizers.Add(_desktopClickRecognizer);
        _desktopClickRecognizer.Tapped += DesktopClickRecognizer_Tapped;

        Content = _layout;
    }

    /// <summary>
    /// Occurs when [on desktop click].
    /// </summary>
    public event EventHandler? OnDesktopClick;

    /// <summary>
    /// Adds the component.
    /// </summary>
    /// <param name="view">The view.</param>
    public void AddComponent(IView view)
    {
        _layout.Children.Add(view);
    }

    /// <summary>
    /// Brings to front.
    /// </summary>
    /// <param name="moduleView">The module view.</param>
    public void BringToFront(IAppModule moduleView)
    {
        _taskbar.BringToFront(moduleView);
    }

    /// <summary>
    /// Starts the desktop.
    /// </summary>
    public async Task StartDesktop()
    {
        await _taskbar.Start(_layout);
    }

    /// <summary>
    /// Starts the module.
    /// </summary>
    /// <param name="module">The module.</param>
    public void StartModule(IAppModule module)
    {
        _layout.Children.Add((IView)module);
    }

    /// <summary>
    /// Stops the specified module.
    /// </summary>
    /// <param name="module">The module.</param>
    public void Stop(IAppModule module)
    {
        _windowService.Stop(module);
    }

    /// <summary>
    /// Stops the module.
    /// </summary>
    /// <param name="module">The module.</param>
    public void StopModule(IAppModule module)
    {
        _layout.Children.Remove((IView)module);
    }

    private void DesktopClickRecognizer_Tapped(object? sender, TappedEventArgs e)
    {
        OnDesktopClick?.Invoke(this, e);
    }
}