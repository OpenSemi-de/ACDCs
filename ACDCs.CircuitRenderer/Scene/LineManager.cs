using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Microsoft.Maui.Graphics;
using static System.Net.Mime.MediaTypeNames;

namespace ACDCs.CircuitRenderer.Scene
{
    public class LineRenderer : IRenderer<LineInstruction>, IRenderer
    {
        public void Render(ICanvas canvas, RenderInstruction instruction, LineInstruction line)
        {
            Coordinate centerPos = new(line.Position);
            DrawableScene.SetStrokeColor(canvas, line.StrokeColor);
            float x = DrawableScene.GetScale(instruction.DrawSize.X, centerPos.X);
            float y = DrawableScene.GetScale(instruction.DrawSize.Y, centerPos.Y);
            canvas.SaveState();
            canvas.DrawLine(
                DrawableScene.GetScale(instruction.DrawSize.X, line.Position.X),
                DrawableScene.GetScale(instruction.DrawSize.Y, line.Position.Y),
                DrawableScene.GetScale(instruction.DrawSize.X, line.End.X),
                DrawableScene.GetScale(instruction.DrawSize.Y, line.End.Y));
            canvas.RestoreState();
        }

    }
}
