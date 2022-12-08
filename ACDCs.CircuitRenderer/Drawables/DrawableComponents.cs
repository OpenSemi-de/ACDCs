using OSECircuitRender.Interfaces;
using OSECircuitRender.Sheet;
using System.Collections.Generic;

namespace OSECircuitRender.Drawables;

public sealed class DrawableComponentList : List<IDrawableComponent>
{
    public DrawableComponentList(Worksheet worksheet)
    {
        Worksheet = worksheet;
    }

    public Worksheet? Worksheet { get; set; }

    public new void Add(IDrawableComponent component)
    {
        component.Worksheet = Worksheet;
        base.Add(component);
    }
}