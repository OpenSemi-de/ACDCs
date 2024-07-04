using ACDCs.Interfaces.Circuit;
using ACDCs.Interfaces.Drawing;
using ACDCs.Shared;
using ACDCs.Structs;
using Rect = Microsoft.Maui.Graphics.Rect;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// Sub renderer class as base for renderers.
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseRenderer<T>
{
    private Point _position;

    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    /// <value>
    /// The position.
    /// </value>
    public Point Position { get => _position; set => _position = value; }

    /// <summary>
    /// Sets the position.
    /// </summary>
    /// <param name="position">The position.</param>
    public void SetPosition(Point position)
    {
        Position = position;
    }
}