﻿using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Scene
{
    public sealed class CircleRenderer : IRenderer, IRenderer<CircleInstruction>
    {
        public void Render(ICanvas canvas, RenderInstruction renderInstruction, CircleInstruction circle) => s_Render(canvas, renderInstruction, circle);

        private static void s_Render(ICanvas canvas, RenderInstruction renderInstruction, CircleInstruction circle)
        {
            Coordinate centerPos = new(circle.Position);
            DrawableScene.SetStrokeColor(canvas, circle.StrokeColor);
            float x = DrawableScene.GetScale(renderInstruction.DrawSize.X, centerPos.X);
            float y = DrawableScene.GetScale(renderInstruction.DrawSize.Y, centerPos.Y);
            canvas.DrawCircle(x, y, renderInstruction.Zoom * renderInstruction.BaseGridSize * 0.1f);
        }
    }
}
