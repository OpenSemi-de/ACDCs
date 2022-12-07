using OSECircuitRender.Interfaces;
using System.Collections.Generic;
using OSECircuitRender.Sheet;

namespace OSECircuitRender.Drawables;

public sealed class DrawableComponentList : List<IDrawableComponent>
{
    public new void Add(IDrawableComponent component)
    {
        component.Worksheet = Worksheet;
        base.Add(component);
    }

    public DrawableComponentList(Worksheet worksheet)
    {
        Worksheet = worksheet;
    }

    public Worksheet? Worksheet { get; set; }
}