using System;
using System.Collections.Generic;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Scene
{
    public static class RenderManager
    {
        private static readonly Dictionary<Type, IRenderer?> s_renderers;

        static RenderManager()
        {
            s_renderers = new Dictionary<Type, IRenderer?>
            {
                { typeof(TextInstruction), new TextRenderer() },
                { typeof(LineInstruction), new LineRenderer() },
                { typeof(CircleInstruction), new CircleRenderer() },
                { typeof(CurveInstruction), new CurveRenderer() },
                { typeof(PathInstruction), new PathRenderer() },
                { typeof(BoxInstruction), new BoxRenderer() },
            };
        }

        public static void Render<T>(ICanvas canvas, RenderInstruction renderInstruction, T instruction)
        {
            if (s_renderers.TryGetValue(typeof(T), out IRenderer? rendererHook))
            {
                if (rendererHook is IRenderer<T> renderer)
                {
                    renderer.Render(canvas, renderInstruction, instruction);
                }
            }
        }
    }
}
