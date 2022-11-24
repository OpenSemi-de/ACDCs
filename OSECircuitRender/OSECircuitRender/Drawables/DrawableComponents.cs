using OSECircuitRender.Interfaces;
using System.Collections.Generic;

namespace OSECircuitRender.Drawables;

public sealed class DrawableComponentList : List<IDrawableComponent>
{
    public DrawableComponentList(IEnumerable<IDrawableComponent> drawables)
    {
        AddRange(drawables);
    }

    public DrawableComponentList()
    {
    }
}