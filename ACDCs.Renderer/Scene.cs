using ACDCs.Interfaces;
using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Drawing;
using ACDCs.Structs;
using Newtonsoft.Json;

namespace ACDCs.Renderer;

/// <summary>
/// The scene class holding the circuits information.
/// </summary>
/// <seealso cref="IScene" />
public class Scene : IScene
{
    /// <summary>
    /// Gets or sets the color of the background.
    /// </summary>
    /// <value>
    /// The color of the background.
    /// </value>
    public Color BackgroundColor { get; set; } = Colors.Transparent;

    /// <summary>
    /// Gets or sets the circuit.
    /// </summary>
    /// <value>
    /// The circuit.
    /// </value>
    public ICircuit Circuit { get; set; } = new Circuit();

    /// <summary>
    /// Gets the click boxes.
    /// </summary>
    /// <value>
    /// The click boxes.
    /// </value>
    [JsonIgnore]
    public List<IClickBox> ClickBoxes { get; set; } = [];

    /// <summary>
    /// Gets or sets the clicked box.
    /// </summary>
    /// <value>
    /// The clicked box.
    /// </value>
    public IClickBox? ClickedBox { get; set; }

    /// <summary>
    /// Gets the debug.
    /// </summary>
    /// <value>
    /// The debug.
    /// </value>
    public SceneDebug Debug { get; set; } = new();

    /// <summary>
    /// Gets the drawings.
    /// </summary>
    /// <value>
    /// The drawings.
    /// </value>
    [JsonIgnore]
    public List<IDrawing> Drawings { get; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether this instance has outline.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has outline; otherwise, <c>false</c>.
    /// </value>
    public bool HasOutline { get; set; } = true;

    /// <summary>
    /// Gets or sets the size of the scene.
    /// </summary>
    /// <value>
    /// The size of the scene.
    /// </value>
    public Rect SceneSize { get; set; } = new Rect(0, 0, 1000, 1000);

    /// <summary>
    /// Gets or sets the size of the step.
    /// </summary>
    /// <value>
    /// The size of the step.
    /// </value>
    public float StepSize { get; set; } = 25.4f * 4;
}