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
    }
}