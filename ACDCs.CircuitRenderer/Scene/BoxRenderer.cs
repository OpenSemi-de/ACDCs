using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Scene;

public class BoxRenderer : IRenderer, IRenderer<BoxInstruction>
{
    public void Render(ICanvas canvas, RenderInstruction renderInstruction, BoxInstruction box)
    {
        Coordinate upperLeft = new(box.Position);
        Coordinate lowerRight = new(box.Size);
        DrawableScene.SetStrokeColor(canvas, renderInstruction.ForegroundColor ?? box.StrokeColor);
        DrawableScene.SetFillColor(canvas, box.FillColor);
        if (box.FillColor != null)
            canvas.FillRectangle(
                DrawableScene.GetScale(renderInstruction.DrawSize.X, upperLeft.X),
                DrawableScene.GetScale(renderInstruction.DrawSize.Y, upperLeft.Y),
                DrawableScene.GetScale(renderInstruction.DrawSize.X, lowerRight.X),
                DrawableScene.GetScale(renderInstruction.DrawSize.Y, lowerRight.Y));

        canvas.DrawRectangle(
            DrawableScene.GetScale(renderInstruction.DrawSize.X, upperLeft.X),
            DrawableScene.GetScale(renderInstruction.DrawSize.Y, upperLeft.Y),
            DrawableScene.GetScale(renderInstruction.DrawSize.X, lowerRight.X),
            DrawableScene.GetScale(renderInstruction.DrawSize.Y, lowerRight.Y));
    }
}
