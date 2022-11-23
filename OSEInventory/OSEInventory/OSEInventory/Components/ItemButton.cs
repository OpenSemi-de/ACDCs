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
            WidthRequest = 42;
            HeightRequest = 42;
            
            if (itemType != null)
            {
                var sheet = wb.AddNewSheet();

                BorderWidth = 1;
                BorderColor = Colors.LightGray;
                sheet.GridSize = 1f;
                sheet.BackgroundColor = new Color(255, 255, 255,  0 );
                sheet.ShowGrid = false;

                if (Activator.CreateInstance(itemType) is WorksheetItem item)
                    sheet.Items.AddItem(item);

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
