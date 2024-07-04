namespace ACDCs.App.GUI.Modules;

using ACDCs.Interfaces.Circuit;
using Sharp.UI;
using ACDCs.Renderer;
using ACDCs.Interfaces.Renderer;
using ACDCs.Shared;
using ACDCs.Interfaces;

/// <summary>
/// A class for a button with a circuit renderer.
/// </summary>
/// <seealso cref="Sharp.UI.Border" />
public class RendererButton : Border
{
    private readonly CircuitRenderer _circuitRenderer;
    private readonly IThemeService _themeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RendererButton" /> class.
    /// </summary>
    /// <param name="component">The component.</param>
    /// <param name="themeService">The theme service.</param>
    public RendererButton(IComponent component, IThemeService themeService)
    {
        _circuitRenderer = (CircuitRenderer)ServiceHelper.GetService<ICircuitRenderer>();
        _circuitRenderer.DisableMovement();

        PointerGestureRecognizer pointerGestureRecognizer = new();
        pointerGestureRecognizer.PointerPressed += PointerGestureRecognizer_PointerPressed;
        _circuitRenderer.GestureRecognizers.Add(pointerGestureRecognizer);

        WidthRequest = 60;
        HeightRequest = 60;

        component.X = -90;
        component.Y = -80;
        Scene scene = new()
        {
            SceneSize = new Rect(0, 0, 60, 60),
        };

        scene.StepSize /= 5;

        scene.Circuit.Components.Add(component);
        scene.BackgroundColor = Colors.White;
        scene.HasOutline = false;
        scene.Debug.ShowClickBoxes = false;
        _circuitRenderer.SetScene(scene);
        Content = _circuitRenderer;
        _themeService = themeService;
    }

    /// <summary>
    /// Occurs when [on clicked].
    /// </summary>
    public event EventHandler? OnClicked;

    /// <summary>
    /// Resets this instance.
    /// </summary>
    public void Reset()
    {
        Color color = _themeService.GetColor(Structs.ColorDefinition.Border);
        Stroke = new SolidColorBrush(color);
    }

    private void PointerGestureRecognizer_PointerPressed(object? sender, PointerEventArgs e)
    {
        OnClicked?.Invoke(this, new());
        Color color = Colors.Black; // _themeService.GetColor(Structs.ColorDefinition.Border);
        Stroke = new SolidColorBrush(color);
    }
}