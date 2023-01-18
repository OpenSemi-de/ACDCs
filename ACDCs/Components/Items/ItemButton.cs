using ACDCs.CircuitRenderer;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Sheet;
using Color = ACDCs.CircuitRenderer.Definitions.Color;

namespace ACDCs.Components.Items;

using Sharp.UI;

public class ItemButton : ImageButton
{
    private IDrawable? _drawableSheet;

    public Type? ItemType { get; set; }

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
        if (ItemType != null)
        {
            Worksheet sheet = new Workbook().AddNewSheet();

            sheet.GridSize = Convert.ToSingle(WidthRequest / Workbook.BaseGridSize * Workbook.Zoom);
            sheet.ShowGrid = false;
            sheet.DisplayOffset = new Coordinate(-10, -10);
            if (BackgroundColor != null)
            {
                sheet.BackgroundColor = new Color(BackgroundColor.WithAlpha(0.2f));
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

            if (sheet.CalculateScene())
            {
                _drawableSheet = sheet.SceneManager?.GetSceneForBackend() as IDrawable;
                if (_drawableSheet != null)
                {
                    using BitmapExportContext context = API.BitmapExportContextService.CreateContext((int)(WidthRequest - 2), (int)(HeightRequest - 2));

                    _drawableSheet?.Draw(context.Canvas, RectF.Zero);

                    using Stream stream = new MemoryStream();
                    context.Image.Save(stream);
                    stream.Position = 0;

                    FakeLocalFile fl = new(stream, "imagebutton_source_" + ItemType.Name + ".bmp");

                    Source = ImageSource.FromFile(fl.FilePath);
                }
            }
        }
    }

    public void SetBackground(Microsoft.Maui.Graphics.Color backgroundColor)
    {
        BackgroundColor = backgroundColor;
    }
}

internal class FakeLocalFile : IDisposable
{
    public string FilePath { get; }

    /// <summary>
    /// Currently ImageSource.FromStream is not working on windows devices.
    /// This class saves the passed stream in a cache directory, returns the local path and deletes it on dispose.
    /// </summary>
    public FakeLocalFile(Stream source, string idFilePath)
    {
        FilePath = System.IO.Path.Combine(FileSystem.Current.CacheDirectory, $"{idFilePath}");
        using FileStream fs = new(FilePath, FileMode.Create);
        source.CopyTo(fs);
    }

    public void Dispose()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}
