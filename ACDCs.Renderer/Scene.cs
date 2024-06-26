using ACDCs.Interfaces;
using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Drawing;
using Newtonsoft.Json;
using Rect = ACDCs.Interfaces.Rect;

namespace ACDCs.Renderer;

/// <summary>
/// The scene class holding the circuits information.
/// </summary>
/// <seealso cref="Interfaces.Circuit.IScene" />
public class Scene : IScene
{
    /// <summary>
    /// Gets or sets the circuit.
    /// </summary>
    /// <value>
    /// The circuit.
    /// </value>
    public ICircuit Circuit { get; set; } = new Circuit();

    /// <summary>
    /// Gets the drawings.
    /// </summary>
    /// <value>
    /// The drawings.
    /// </value>
    [JsonIgnore]
    public List<IDrawing> Drawings { get; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether this instance has outline (default:true).
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
    public float StepSize { get; set; } = 25.4f;
}