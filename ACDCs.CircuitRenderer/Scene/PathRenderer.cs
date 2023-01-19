using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Scene;

public sealed class PathRenderer : IRenderer, IRenderer<PathInstruction>
{
    public void Render(ICanvas canvas, RenderInstruction renderInstruction, PathInstruction path) => s_Render(canvas, renderInstruction, path);

    private static void s_Render(ICanvas canvas, RenderInstruction renderInstruction, PathInstruction path)
    {
        PathF pathF = new();
        DrawableScene.SetStrokeColor(canvas, renderInstruction.ForegroundColor ?? path.StrokeColor);

        float scaleX = renderInstruction.DrawSize.X / path.Width;
        float scaleY = renderInstruction.DrawSize.Y / path.Height / 2;
        foreach (PathPart part in path.GetParts())
            switch (part.Type)
            {
                case PathPartType.A:
                    break;

                case PathPartType.C:
                    {
                        pathF.CurveTo(
                            part.Coordinates[0].X * scaleX,
                            part.Coordinates[0].Y * scaleY,
                            part.Coordinates[1].X * scaleX,
                            part.Coordinates[1].Y * scaleY,
                            part.Coordinates[2].X * scaleX,
                            part.Coordinates[2].Y * scaleY
                        );
                    }
                    break;

                case PathPartType.M:
                    {
                        pathF.MoveTo(
                            part.Coordinates[0].X * scaleX,
                            part.Coordinates[0].Y * scaleY
                        );
                    }
                    break;

                case PathPartType.L:
                    {
                        pathF.LineTo(
                            part.Coordinates[0].X * scaleX,
                            part.Coordinates[0].Y * scaleY
                        );
                    }
                    break;

                case PathPartType.H:
                    break;

                case PathPartType.V:
                    break;

                case PathPartType.S:
                    break;

                case PathPartType.Q:
                    break;

                case PathPartType.T:
                    break;

                case PathPartType.Z:
                    break;
            }

        canvas.DrawPath(pathF);
    }
}
