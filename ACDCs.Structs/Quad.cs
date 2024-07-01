namespace ACDCs.Structs;

/// <summary>
/// Quad class. Order: LU, RU, RL, LL
/// </summary>
public class Quad
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Quad"/> class.
    /// </summary>
    public Quad()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Quad"/> class.
    /// </summary>
    /// <param name="x1">The x1.</param>
    /// <param name="y1">The y1.</param>
    /// <param name="x2">The x2.</param>
    /// <param name="y2">The y2.</param>
    /// <param name="x3">The x3.</param>
    /// <param name="y3">The y3.</param>
    /// <param name="x4">The x4.</param>
    /// <param name="y4">The y4.</param>
    public Quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
        X3 = x3;
        Y3 = y3;
        X4 = x4;
        Y4 = y4;
    }

    /// <summary>
    /// Gets or sets the x1.
    /// </summary>
    /// <value>
    /// The x1.
    /// </value>
    public float X1 { get; set; }

    /// <summary>
    /// Gets or sets the x2.
    /// </summary>
    /// <value>
    /// The x2.
    /// </value>
    public float X2 { get; set; }

    /// <summary>
    /// Gets or sets the x3.
    /// </summary>
    /// <value>
    /// The x3.
    /// </value>
    public float X3 { get; set; }

    /// <summary>
    /// Gets or sets the x4.
    /// </summary>
    /// <value>
    /// The x4.
    /// </value>
    public float X4 { get; set; }

    /// <summary>
    /// Gets or sets the y1.
    /// </summary>
    /// <value>
    /// The y1.
    /// </value>
    public float Y1 { get; set; }

    /// <summary>
    /// Gets or sets the y2.
    /// </summary>
    /// <value>
    /// The y2.
    /// </value>
    public float Y2 { get; set; }

    /// <summary>
    /// Gets or sets the y3.
    /// </summary>
    /// <value>
    /// The y3.
    /// </value>
    public float Y3 { get; set; }

    /// <summary>
    /// Gets or sets the y4.
    /// </summary>
    /// <value>
    /// The y4.
    /// </value>
    public float Y4 { get; set; }
}