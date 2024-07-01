using ACDCs.Interfaces.Drawing;
using ACDCs.Structs;
using System.Text.Json.Serialization;

namespace ACDCs.Interfaces.Circuit;

using Rect = Microsoft.Maui.Graphics.Rect;

/// <summary>
/// The interface to the scene class.
/// </summary>
public interface IScene
{
    /// <summary>
    /// Gets or sets the color of the background.
    /// </summary>
    /// <value>
    /// The color of the background.
    /// </value>
    public Color BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the circuit.
    /// </summary>
    /// <value>
    /// The circuit.
    /// </value>
    public ICircuit Circuit { get; set; }

    /// <summary>
    /// Gets the click boxes.
    /// </summary>
    /// <value>
    /// The click boxes.
    /// </value>
    public List<Quad> ClickBoxes { get; }

    /// <summary>
    /// Gets the debug.
    /// </summary>
    /// <value>
    /// The debug.
    /// </value>
    public SceneDebug Debug { get; }

    /// <summary>
    /// Gets the drawings.
    /// </summary>
    /// <value>
    /// The drawings.
    /// </value>
    [JsonIgnore]
    public List<IDrawing> Drawings { get; }

    /// <summary>
    /// Gets or sets the size of the scene.
    /// </summary>
    /// <value>
    /// The size of the scene.
    /// </value>
    public Rect SceneSize { get; set; }

    /// <summary>
    /// Gets or sets the size of the step.
    /// </summary>
    /// <value>
    /// The size of the step.
    /// </value>
    public float StepSize { get; set; }
}