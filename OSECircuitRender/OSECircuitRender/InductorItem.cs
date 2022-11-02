namespace OSECircuitRender
{
    public sealed class InductorItem : IWorksheetItem
    {
        public InductorItem()
        {
            DrawableComponent = new InductorDrawable(this);
        }

        public InductorItem(string value, float x, float y)
        {
            DrawableComponent = new InductorDrawable(this, value, x, y);
        }

        public string RefName { get; set; }
        public IDrawableComponent DrawableComponent { get; set; }
    }
}