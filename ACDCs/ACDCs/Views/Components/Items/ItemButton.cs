using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using Microsoft.Maui.Storage;
using OSECircuitRender;
using OSECircuitRender.Definitions;
using OSECircuitRender.Items;
using OSECircuitRender.Sheet;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Color = OSECircuitRender.Definitions.Color;

namespace ACDCs.Views.Components.Items
{
    public class ItemButton : ImageButton
    {
        private readonly IDrawable? _drawableSheet;

        public ItemButton(Type? itemType)
        {
            ItemType = itemType;

            if (itemType != null)
            {
                Worksheet sheet = new Workbook().AddNewSheet();

                sheet.GridSize = Convert.ToSingle(WidthRequest / Workbook.BaseGridSize * Workbook.Zoom);
                sheet.BackgroundColor = new Color(255, 255, 255, 0);
                sheet.ShowGrid = false;
                sheet.DisplayOffset = new Coordinate(-10, -10);

                object?[] arguments = { };
                if (Activator.CreateInstance(itemType, args: arguments) is WorksheetItem item)
                {
                    sheet.Items.AddItem(item);
                    sheet.GridSize = 3f / item.Width;
                    sheet.DisplayOffset.Y = 7 * (3 / item.Height - 1) - 7;
                }

                if (sheet.CalculateScene())
                {
                    _drawableSheet = sheet.SceneManager?.GetSceneForBackend() as IDrawable;
                    if (_drawableSheet != null)
                    {
                        using SkiaBitmapExportContext context = new(42, 42, 1);

                        _drawableSheet?.Draw(context.Canvas, RectF.Zero);

                        using Stream stream = new MemoryStream();
                        context.Image.Save(stream);
                        stream.Position = 0;

                        FakeLocalFile fl = new(stream, "imagebutton_source_" + itemType.Name + ".bmp");

                        Source = ImageSource.FromFile(fl.FilePath);
                    }
                }
            }
        }

        public Type? ItemType { get; set; }

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
}