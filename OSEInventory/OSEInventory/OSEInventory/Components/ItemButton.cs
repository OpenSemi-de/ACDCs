using Microsoft.Maui.Graphics.Skia;
using OSECircuitRender;
using OSECircuitRender.Definitions;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using Color = OSECircuitRender.Definitions.Color;

namespace OSEInventory.Components
{
    public class ItemButton : ImageButton
    {
        private static Workbook wb = new();
        private readonly IDrawable? drawableSheet;

        public ItemButton(Type? itemType)
        {
            ItemType = itemType;
            if (WidthRequest == 0)
                WidthRequest = 42;
            if (HeightRequest == 0)
                HeightRequest = 42;

            if (itemType != null)
            {
                var sheet = wb.AddNewSheet();

                BorderWidth = 2;
                BorderColor = Colors.WhiteSmoke;
                sheet.GridSize = Convert.ToSingle(WidthRequest / Workbook.BaseGridSize * Workbook.Zoom);
                sheet.BackgroundColor = new Color(255, 255, 255, 40);
                sheet.ShowGrid = false;
                sheet.DisplayOffset = new Coordinate(-10, -10);

                if (Activator.CreateInstance(itemType) is WorksheetItem item)
                {
                    sheet.Items.AddItem(item);
                    sheet.GridSize = 3f / item.Width;
                    sheet.DisplayOffset.Y = 7 * (3 / item.Height - 1) - 7;
                }

                if (sheet.CalculateScene())
                {

                    drawableSheet = sheet.SceneManager.GetSceneForBackend() as IDrawable;
                    if (drawableSheet != null)
                    {

                        using SkiaBitmapExportContext context = new(42, 42, 1);

                        drawableSheet?.Draw(context.Canvas, RectF.Zero);

                        using (Stream stream = new MemoryStream())
                        {
                            context.Image.Save(stream);
                            stream.Position = 0;


                            FakeLocalFile fl = new(stream, "imagebutton_source_" + itemType.Name + ".bmp");

                            Source = ImageSource.FromFile(fl.FilePath);
                        }

                    }
                }

            }
        }

        private Task<Stream> StreamImage(CancellationToken arg)
        {
            using SkiaBitmapExportContext context = new(40, 40, 1);

            drawableSheet?.Draw(context.Canvas, RectF.Zero);

            using Stream stream = new MemoryStream();

            context.WriteToStream(stream);

            return Task.FromResult(stream);
        }

        public Type? ItemType { get; set; }
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
            FilePath = Path.Combine(FileSystem.Current.CacheDirectory, $"{idFilePath}");
            using var fs = new FileStream(FilePath, FileMode.Create);
            source.CopyTo(fs);
        }

        public void Dispose()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }


}
