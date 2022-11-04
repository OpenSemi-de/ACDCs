using System.Collections.Generic;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Drawables
{
    public sealed class DrawableComponentList : List<IDrawableComponent>
    {
        public DrawableComponentList(IEnumerable<IDrawableComponent> drawables)
        {
            AddRange(drawables);
        }
    }
}