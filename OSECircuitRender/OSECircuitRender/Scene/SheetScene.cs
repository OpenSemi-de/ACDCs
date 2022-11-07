using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;

namespace OSECircuitRender.Scene
{
    public sealed class SheetScene
    {
        public DrawableComponentList Drawables { get; set; }

        public bool SetDrawables(DrawableComponentList drawables)
        {
            Drawables = drawables;
            return true;
        }

        public bool ShowGrid { get; set; }
        public float GridSize { get; set; }
        public Coordinate SheetSize { get; set; }
    }
}