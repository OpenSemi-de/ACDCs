using System.Collections.Generic;

namespace OSECircuitRender
{
    public sealed class DrawableComponents : List<IDrawableComponent>
    {
        public DrawableComponents(IEnumerable<IDrawableComponent> drawables)
        {
            base.AddRange(drawables);
        }
    }
}