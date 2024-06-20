using ACDCs.Interfaces;
using ACDCs.Renderer.Drawings;
using ACDCs.Renderer.Managers;

namespace ACDCs.Renderer.Renderers;

/// <summary>
/// The line renderer.
/// </summary>
/// <seealso cref="ACDCs.Interfaces.IRenderer" />
/// <seealso cref="ACDCs.Interfaces.ITextRenderer" />
public class LineRenderer : SubRenderer, IRenderer, ITextRenderer
{
    /// <summary>
    /// Gets the type of the drawing.
    /// </summary>
    /// <value>
    /// The type of the drawing.
    /// </value>
    public override Type DrawingType { get => typeof(LineDrawing); }

    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void Draw(ICanvas canvas)
    {
        RenderSettingsManager.ApplyColors(canvas);

        foreach (TextDrawing text in Drawings)
        {
            float x = text.X;
            float y = text.Y;
            float width = text.Width;
            float height = text.Height;

            if (!text.IsRelativeScale)
            {
                x += Convert.ToSingle(Position.X);
                y += Convert.ToSingle(Position.Y);
            }
            else
            {
                x = Convert.ToSingle(text.ParentDrawing.X + Position.X + (Scene?.StepSize * text.X));
                y = Convert.ToSingle(text.ParentDrawing.Y + Position.Y + (Scene?.StepSize * text.Y));
                width = Convert.ToSingle(text.ParentDrawing.Width * Scene?.StepSize);
                height = Convert.ToSingle(height * Scene?.StepSize);
            }

            canvas.DrawString(text.Text, x, y, width, height, HorizontalAlignment.Center, VerticalAlignment.Center);
        }
    }
}
