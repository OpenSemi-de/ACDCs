namespace OSECircuitRender
{
    public sealed class SheetScene
    {
        public DrawableComponents Drawables { get; set; }

        public bool SetDrawables(DrawableComponents drawables)
        {
            Drawables = drawables;
            return true;
        }
    }
}