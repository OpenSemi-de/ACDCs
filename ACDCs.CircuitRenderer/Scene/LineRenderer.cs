﻿using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Scene;

public sealed class LineRenderer : IRenderer<LineInstruction>, IRenderer
{
    public void Render(ICanvas canvas, RenderInstruction renderInstruction, LineInstruction line) => s_Render(canvas, renderInstruction, line);

    private static void s_Render(ICanvas canvas, RenderInstruction renderInstruction, LineInstruction line)
    {
        canvas.SaveState();

        Coordinate centerPos = new(line.Position);
        DrawableScene.SetStrokeColor(canvas,
            Equals(line.StrokeColor?.ToMauiColor(), Colors.Black) ? renderInstruction.ForegroundColor : line.StrokeColor);
        DrawableScene.SetStrokeWidth(canvas, line.StrokeWidth);

        canvas.DrawLine(
            DrawableScene.GetScale(renderInstruction.DrawSize.X, line.Position.X),
            DrawableScene.GetScale(renderInstruction.DrawSize.Y, line.Position.Y),
            DrawableScene.GetScale(renderInstruction.DrawSize.X, line.End.X),
            DrawableScene.GetScale(renderInstruction.DrawSize.Y, line.End.Y));

        canvas.RestoreState();
    }
}
