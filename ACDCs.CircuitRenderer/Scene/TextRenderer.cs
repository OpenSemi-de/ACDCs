using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Scene;

public class TextRenderer : IRenderer<TextInstruction>, IRenderer
{
    public void Render(ICanvas canvas, RenderInstruction instruction, TextInstruction text)
    {
        Coordinate centerPos = new(text.Position);
        DrawableScene.SetStrokeColor(canvas, text.StrokeColor);
        canvas.FontColor = instruction.ForegroundColor != null ? instruction.ForegroundColor.ToMauiColor() : text.StrokeColor?.ToMauiColor();

        float x = DrawableScene.GetScale(instruction.DrawSize.X, centerPos.X);
        float y = DrawableScene.GetScale(instruction.DrawSize.Y, centerPos.Y);
        canvas.SaveState();

        if (text.IsRealFontSize)
        {
            canvas.FontSize = text.FontSize;
        }
        else
        {
            canvas.FontSize = instruction.FontSize / text.FontSize * 12;
        }

        canvas.Translate(x, y);
        canvas.Rotate(text.Orientation);
        canvas.DrawString(text.Text, 0, 0, HorizontalAlignment.Center);
        canvas.RestoreState();
    }
}
