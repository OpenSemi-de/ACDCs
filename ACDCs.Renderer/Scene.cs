using ACDCs.Interfaces;
using Newtonsoft.Json;
using Rect = ACDCs.Interfaces.Rect;

namespace ACDCs.Renderer;

/// <summary>
/// The scene class holding the circuits information.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IScene" />
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
    /// Gets or sets the size of the scene.
    /// </summary>
    /// <value>
    /// The size of the scene.
    /// </value>
    public Rect SceneSize { get; set; } = new Rect(0, 0, 1000, 1000);
}