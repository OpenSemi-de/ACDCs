using ACDCs.CircuitRenderer.Scene;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Interfaces
{
    public interface IRenderer
    {

    }

    public interface IRenderer<T>
    {
        public void Render(ICanvas canvas, RenderInstruction renderInstruction, T instruction);

    }
}
