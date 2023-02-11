using Font = Microsoft.Maui.Graphics.Font;

namespace ACDCs.ApplicationLogic.Services;

using Interfaces;
using Sharp.UI;

public class ImageService : IImageService
{
    public ImageSource? BackgroundImageSource(ContentPage view)
    {
        return BackgroundImageSource((float)view.Width, (float)view.Height);
    }

    public ImageSource? BackgroundImageSource(float width, float height)
    {
        try
        {
            using BitmapExportContext context = API.BitmapExportContextService.CreateContext((int)width, (int)height);
            ICanvas? canvas = context.Canvas;

            List<Color> colors = new() { API.Instance.Full, API.Instance.Background };

            canvas.SetShadow(new SizeF(6, 8), 10f, API.Instance.BackgroundHigh);

            RadialGradientPaint paintFrom = new(new Point(1, 1), 1) { StartColor = colors[0], EndColor = colors[1] };

            canvas.SetFillPaint(paintFrom, new RectF(0f, 0f, width, height));
            canvas.FillRectangle(0, 0, width, height);

            RadialGradientPaint paintTo =
                new(new Point(1, 1), 1) { StartColor = API.Instance.Foreground, EndColor = colors[0] };

            canvas.SetFillPaint(paintTo, new RectF(0f, 0f, width, height));
            canvas.FontSize = 120;
            canvas.Font = new Font("Maple Mono");
            canvas.FontColor = paintTo.ToColor();
            canvas.DrawString("ACDCs", 0, 0, width, height, HorizontalAlignment.Center, VerticalAlignment.Center);

            ImageSource source = GetImageSource(context).GetAwaiter().GetResult();

            return source;
        }
        catch
        {
            // ignored
        }

        return null;
    }

    public ImageSource? BackgroundImageSource(View view)
    {
        return BackgroundImageSource(Convert.ToSingle(view.Width), Convert.ToSingle(view.Height));
    }

    public ImageSource? ButtonImageSource(string text, int width, int height)
    {
        try
        {
            using BitmapExportContext context = API.BitmapExportContextService.CreateContext(width, height);
            ICanvas? canvas = context.Canvas;
            canvas.Alpha = 0.7f;

            List<Color> colors = new() { API.Instance.Background, API.Instance.BackgroundHigh };

            canvas.SetShadow(new SizeF(2, 4), 10f, colors[1]);

            LinearGradientPaint paintFrom = new(Point.Zero, new Point(1, 1))
            {
                StartColor = colors[0],
                EndColor = colors[1]
            };
            canvas.SetFillPaint(paintFrom, new RectF(0f, 0f, width, height));
            canvas.FillRoundedRectangle(0, 0, width, height, 2);
            canvas.StrokeSize = 3;
            canvas.StrokeColor = API.Instance.Border;
            canvas.Antialias = false;
            canvas.DrawRoundedRectangle(2, 2, width - 4, height - 4, 2);
            canvas.Antialias = true;

            canvas.FontColor = API.Instance.Text;
            canvas.Font = new Font("Maple Mono");
            canvas.DrawString(text, width / 2, height / 2, HorizontalAlignment.Center);

            ImageSource source = GetImageSource(context).GetAwaiter().GetResult();

            return source;
        }
        catch
        {
            // ignored
        }

        return null;
    }

    private static async Task<ImageSource> GetImageSource(BitmapExportContext context)
    {
        MemoryStream? ms = new();

        await context.Image.SaveAsync(ms);
        ms.Position = 0;

        ImageSource? source = ImageSource.FromStream(() =>
        {
            return GetStream(ms);
        });
        return source;
    }

    private static Stream GetStream(MemoryStream memoryStream)
    {
#pragma warning disable CS4014
        Task.Run(() =>
#pragma warning restore CS4014
        {
            Task.Delay(10000).Wait();
            memoryStream.Dispose();
            GC.Collect();
        });

        return memoryStream;
    }

    public ImageSource? WindowImageSource(float width, float height)
    {
        try
        {
            using BitmapExportContext context = API.BitmapExportContextService.CreateContext((int)width, (int)height);
            ICanvas? canvas = context.Canvas;

            List<Color> colors = new() { API.Instance.Background, API.Instance.Foreground };

            RadialGradientPaint paintFrom = new(new Point(1, 0), 1)
            {
                StartColor = colors[0],
                EndColor = colors[1]
            };

            canvas.SetFillPaint(paintFrom, new RectF(0f, 0f, width, height));
            canvas.FillRoundedRectangle(0, 0, width, height, 1);
            canvas.StrokeSize = 1;
            canvas.StrokeColor = API.Instance.Border;
            canvas.Antialias = false;
            canvas.DrawRoundedRectangle(2, 2, width - 4, height - 4, 1);

            canvas.Antialias = true;
            canvas.FillRoundedRectangle(0, 0, width, 38, 1);
            canvas.Antialias = false;
            canvas.DrawRoundedRectangle(2, 2, width - 4, 36, 1);

            ImageSource source = GetImageSource(context).GetAwaiter().GetResult();

            return source;
        }
        catch
        {
            // ignored
        }

        return null;
    }
}
