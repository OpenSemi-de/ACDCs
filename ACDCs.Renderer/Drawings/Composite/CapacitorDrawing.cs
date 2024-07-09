using ACDCs.Interfaces;
using ACDCs.Interfaces.Drawing;
using ACDCs.Shared;
using ACDCs.Structs;

namespace ACDCs.Renderer.Drawings.Composite;

/// <summary>
/// Drawing for a capacitor element.
/// </summary>
/// <seealso cref="BaseDrawing" />
/// <seealso cref="ICompositeDrawing" />
/// <seealso cref="IDrawingWithSize" />
public class CapacitorDrawing : BaseDrawing, ICompositeDrawing, IDrawingWithSize
{
    private readonly IThemeService _themeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CapacitorDrawing"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="value">The value.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="rotation">The rotation.</param>
    /// <param name="isPolar">if set to <c>true</c> [is polar].</param>
    public CapacitorDrawing(string id, float value, float x, float y, float rotation, bool isPolar = true)
    {
        Id = id;
        Rotation = rotation;
        IsPolar = isPolar;
        Value = value;
        X = x;
        Y = y;
        _themeService = ServiceHelper.GetService<IThemeService>();
    }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height { get; set; } = 1;

    /// <summary>
    /// Gets a value indicating whether this instance is polar.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is polar; otherwise, <c>false</c>.
    /// </value>
    public bool IsPolar { get; set; }

    /// <summary>
    /// Gets or sets the offset.
    /// </summary>
    /// <value>
    /// The offset.
    /// </value>
    public Point Offset { get; set; } = new Point(0, -0.5);

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float Width { get; set; } = 2;

    /// <summary>
    /// Gets the drawings.
    /// </summary>
    /// <returns></returns>
    public List<IDrawing> GetDrawings()
    {
        List<IDrawing> drawings =
        [
            new PointDrawing(Id + "_Pin1", -0.05f, 0.45f, 0.05f, 0.55f, true),
            new LineDrawing(Id + "_Line1", 0.05f, 0.5f, 0.45f, 0.5f, true),
            new BoxDrawing(Id + "_Line2", 0.45f, 0.2f, 0.05f, 0.6f, true),
            new ArcDrawing(Id + "_Arc1", 0.55f, 0.2f, 0.2f, 0.6f, 120, 240, true).SetStrokeSize(4).SetBackgroundColor(_themeService.GetColor(ColorDefinition.CircuitRendererStroke)),
            new LineDrawing(Id + "_Line3", 0.55f, 0.5f, 0.95f, 0.5f, true).SetStrokeSize(4),
            new PointDrawing(Id + "_Pin2", 0.95f, 0.45f, 1.05f, 0.55f, true),
            new TextDrawing(Id + "_Text", Value.ToString(), 0f, 0.8f, 1f, 0.4f, 0, true)
        ];

        foreach (IDrawing drawing in drawings)
        {
            drawing.ParentDrawing = this;
        }

        return drawings;
    }
}