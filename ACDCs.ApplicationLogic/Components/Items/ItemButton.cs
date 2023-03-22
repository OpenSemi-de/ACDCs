namespace ACDCs.API.Core.Components.Items;

using CircuitRenderer;
using CircuitRenderer.Definitions;
using CircuitRenderer.Items;
using CircuitRenderer.Sheet;
using Instance;
using Color = Color;

public class ItemButton : ImageButton
{
    private IDrawable? _drawableSheet;

    public Type? ItemType { get; }

    public ItemButton(Type? itemType, double buttonWidth, double buttonHeight)
    {
        ItemType = itemType;

        this.Margin(new Thickness(0))
            .Padding(new Thickness(2))
            .CornerRadius(4)
            .WidthRequest(buttonWidth)
            .HeightRequest(buttonHeight)
            .BackgroundColor(Colors.Transparent)
            .BorderWidth(0);
    }

    public void Draw()
    {
        if (ItemType == null)
        {
            return;
        }

        Worksheet sheet = new Workbook().AddNewSheet();

        sheet.GridSize = Convert.ToSingle(WidthRequest / Workbook.BaseGridSize * Workbook.Zoom);
        sheet.ShowGrid = false;
        sheet.DisplayOffset = new Coordinate(-10, -10);
        if (BackgroundColor != null)
        {
            sheet.BackgroundColor = new CircuitRenderer.Definitions.Color(BackgroundColor.WithAlpha(0.2f));
            BackgroundColor = BackgroundColor.WithAlpha(0.2f);
        }

        object?[] arguments = { };
        if (Activator.CreateInstance(ItemType, args: arguments) is WorksheetItem item)
        {
            sheet.Items.AddItem(item);
            sheet.GridSize = (float)(3f / item.Width * (HeightRequest / 46f));
            if (item.Width != 1)
                sheet.DisplayOffset.Y = 7 * (3 / item.Height - 1) - 7;
            else
                sheet.DisplayOffset.Y = -20;
        }

        if (!sheet.CalculateScene())
        {
            return;
        }

        _drawableSheet = sheet.SceneManager?.GetSceneForBackend() as IDrawable;
        if (_drawableSheet == null)
        {
            return;
        }

        using BitmapExportContext context = API.BitmapExportContextService.CreateContext((int)(WidthRequest - 2), (int)(HeightRequest - 2));

        _drawableSheet?.Draw(context.Canvas, RectF.Zero);

        using Stream stream = new MemoryStream();
        context.Image.Save(stream);
        stream.Position = 0;

        FakeLocalFile fl = new(stream, "imagebutton_source_" + ItemType.Name + ".bmp");

        Source = ImageSource.FromFile(fl.FilePath);
    }

    public void SetBackground(Color backgroundColor)
    {
        BackgroundColor = backgroundColor;
    }
}
