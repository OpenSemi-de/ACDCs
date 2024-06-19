using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items;

public class TextItem : WorksheetItem
{
    public override string DefaultValue => "Text";

    public override bool IsInsertable => true;

    public bool IsRealFontSize
    {
        get => ((TextDrawable)DrawableComponent).IsRealFontSize;
        set => ((TextDrawable)DrawableComponent).IsRealFontSize = value;
    }

    public TextItem()
    {
        DrawableComponent = new TextDrawable(this, DefaultValue, 12f, 1, 1);
        Value = DefaultValue;
    }

    public TextItem(string value, float textSize, float x, float y)
    {
        DrawableComponent = new TextDrawable(this, value, textSize, x, y);
        Value = value;
    }
}
