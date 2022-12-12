using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Scene
{
    public class CurveRenderer : IRenderer, IRenderer<CurveInstruction>
    {
        public void Render(ICanvas canvas, RenderInstruction renderInstruction, CurveInstruction curve) 
        {
            Coordinate centerPos = new(curve.Position);
            DrawableScene.SetStrokeColor(canvas, curve.StrokeColor);

            float startX = DrawableScene.GetScale(renderInstruction.DrawSize.X, curve.Position.X);
            float startY = DrawableScene.GetScale(renderInstruction.DrawSize.Y, curve.Position.Y);
            float width = DrawableScene.GetScale(renderInstruction.DrawSize.X, curve.End.X) - startX;
            float height = DrawableScene.GetScale(renderInstruction.DrawSize.Y, curve.End.Y) - startY;

            canvas.DrawArc(
                startX,
                startY,
                width,
                height,
                curve.AngleStart,
                curve.AngleEnd, false, false);

        }
    }
}
