using OSECircuitRender.Interfaces;
using System.Collections.Generic;
using System.Linq;
using OSECircuitRender.Sheet;

namespace OSECircuitRender.Drawables;

public sealed class DrawableComponentList : List<IDrawableComponent>
{
    public DrawableComponentList(IEnumerable<IDrawableComponent> drawables)
    {
        var drawableComponents = drawables.ToList();
        foreach (var drawableComponent in drawableComponents)
        {
            drawableComponent.Worksheet = Worksheet;
        }

        AddRange(drawableComponents);
    }

    public new void Add(IDrawableComponent component)
    {
        component.Worksheet = Worksheet;
        base.Add(component);
    }

    public DrawableComponentList()
    {
    }

    public Worksheet? Worksheet { get; set; }
}