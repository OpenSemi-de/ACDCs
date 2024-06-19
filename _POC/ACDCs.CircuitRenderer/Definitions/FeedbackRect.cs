using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Items;
using Microsoft.Maui.Graphics;

namespace ACDCs.CircuitRenderer.Definitions;

public class FeedbackRect
{
    public DrawableComponent? Drawable { get; set; }

    public bool IsSelected { get; set; }

    public WorksheetItem? Item { get; set; }

    public RectF? Rect { get; set; }

    public FeedbackRect(bool isSelected, DrawableComponent? drawable)
    {
        IsSelected = isSelected;
        Drawable = drawable;
    }
}
