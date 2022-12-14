using ACDCs.CircuitRenderer;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Sheet;
using Microsoft.Maui.Graphics.Skia;
using Color = ACDCs.CircuitRenderer.Definitions.Color;

namespace ACDCs.Views.Components.Items;

public class ItemButton : ImageButton
{
    public ItemButton(Type? itemType)
    {
        ItemType = itemType;
        Padding = 0;
    }

    public Type? ItemType { get; set; }

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
                    using SkiaBitmapExportContext context = new((int)(WidthRequest - 2), (int)(HeightRequest - 2), 1);

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

    private IDrawable? _drawableSheet;

    private Task<Stream> StreamImage(CancellationToken arg)
    {
        using SkiaBitmapExportContext context = new(40, 40, 1);

        _drawableSheet?.Draw(context.Canvas, RectF.Zero);

        using Stream stream = new MemoryStream();

        context.WriteToStream(stream);

        return Task.FromResult(stream);
    }
}

internal class FakeLocalFile : IDisposable
{
    /// <summary>
    /// Currently ImageSource.FromStream is not working on windows devices.
    /// This class saves the passed stream in a cache directory, returns the local path and deletes it on dispose.
    /// </summary>
    public FakeLocalFile(Stream source, string idFilePath)
    {
        FilePath = Path.Combine(FileSystem.Current.CacheDirectory, $"{idFilePath}");
        using FileStream fs = new(FilePath, FileMode.Create);
        source.CopyTo(fs);
    }

    public string FilePath { get; }

    public void Dispose()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}
