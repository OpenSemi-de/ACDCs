using ACDCs.Interfaces;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The text renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class TextRenderer : IRenderer, ITextRenderer
{
    private Microsoft.Maui.Graphics.Point _position;
    private IScene? _scene;
    private List<IDrawing> _textDrawings = [];

    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        RenderColorManager.ApplyColors(canvas);

        foreach (TextDrawing text in _textDrawings)
        {
            float x = text.X + Convert.ToSingle(_position.X);
            float y = text.Y + Convert.ToSingle(_position.Y);
            canvas.DrawString(text.Text, x, y, text.Width, text.Height, HorizontalAlignment.Center, VerticalAlignment.Center);
        }
    }

    /// <summary>
    /// Sets the position.
    /// </summary>
    /// <param name="position">The position.</param>
    public void SetPosition(Microsoft.Maui.Graphics.Point position)
    {
        _position = position;
    }

    /// <summary>
    /// Sets the scene.
    /// </summary>
    /// <param name="scene">The scene.</param>
    public void SetScene(IScene scene)
    {
        _scene = scene;
        _textDrawings = scene.Drawings.Where(d => d.GetType() == typeof(TextDrawing)).ToList();
    }
}