using ACDCs.Interfaces;
using ACDCs.Renderer.Drawings;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The text renderer.
/// </summary>
/// <seealso cref="IRenderer" />
/// <seealso cref="ITextRenderer" />
public class TextRenderer : IRenderer, ITextRenderer
{
    private IScene? _scene;
    private List<IDrawing> _textDrawings = [];

    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        foreach (TextDrawing text in _textDrawings)
        {
            canvas.DrawString(text.Text, text.X, text.Y, HorizontalAlignment.Left);
        }
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