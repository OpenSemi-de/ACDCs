using ACDCs.Interfaces;
using ACDCs.Interfaces.Drawing;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Base Drawing class.
/// </summary>
public class BaseDrawing : IDrawing
{
    /// <summary>
    /// Gets or sets the color of the background.
    /// </summary>
    /// <value>
    /// The color of the background.
    /// </value>
    public Color? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public string Id { get; set; } = "";

    /// <summary>
    /// Gets or sets a value indicating whether this instance is relative scale.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is relative scale; otherwise, <c>false</c>.
    /// </value>
    public bool IsRelativeScale { get; set; }

    /// <summary>
    /// Gets or sets the color of the line.
    /// </summary>
    /// <value>
    /// The color of the line.
    /// </value>
    public Color? LineColor { get; set; }

    /// <summary>
    /// Gets or sets the parent drawing.
    /// </summary>
    /// <value>
    /// The parent drawing.
    /// </value>
    public IDrawing? ParentDrawing { get; set; }

    /// <summary>
    /// Gets or sets the rotation.
    /// </summary>
    /// <value>
    /// The rotation.
    /// </value>
    public float Rotation { get; set; }

    /// <summary>
    /// Gets or sets the size of the stroke.
    /// </summary>
    /// <value>
    /// The size of the stroke.
    /// </value>
    public float StrokeSize { get; set; } = 2;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public float Value { get; set; }

    /// <summary>
    /// Gets or sets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public float X { get; set; }

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public float Y { get; set; }

    /// <summary>
    /// Sets the color of the background.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IDrawing SetBackgroundColor(Color color)
    {
        BackgroundColor = color;
        return this;
    }

    /// <summary>
    /// Sets the size of the stroke.
    /// </summary>
    /// <param name="strokeSize">Size of the stroke.</param>
    /// <returns></returns>
    public IDrawing SetStrokeSize(float strokeSize)
    {
        StrokeSize = strokeSize;
        return this;
    }
}