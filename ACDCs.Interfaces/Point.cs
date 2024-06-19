namespace ACDCs.Interfaces;

/// <summary>
/// A json serializable Podouble object.
/// </summary>
public class Point
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> class.
    /// </summary>
    public Point()
    {
        X = 0;
        Y = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> class.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Gets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public double X { get; }

    /// <summary>
    /// Gets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public double Y { get; }
}

/// <summary>
/// A json seriazable Rect.
/// </summary>
public class Rect
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Rect"/> class.
    /// </summary>
    public Rect()
    {
        X = 0;
        Y = 0;
        Width = 0;
        Height = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Point" /> class.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public Rect(double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public double Height { get; }

    /// <summary>
    /// Gets the width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public double Width { get; }

    /// <summary>
    /// Gets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public double X { get; }

    /// <summary>
    /// Gets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public double Y { get; }
}