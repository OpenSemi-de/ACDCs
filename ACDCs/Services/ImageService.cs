using Font = Microsoft.Maui.Graphics.Font;

namespace ACDCs.Services;

public static class ImageService
{
    public static ImageSource? BackgroundImageSource(IView view)
    {
        return BackgroundImageSource(Convert.ToSingle(view.Width), Convert.ToSingle(view.Height));
    }

    public static ImageSource? BackgroundImageSource(ContentPage view)
    {
        return BackgroundImageSource((float)view.Width, (float)view.Height);
    }

    public static ImageSource? BackgroundImageSource(float width, float height)
    {
        try
        {
            using BitmapExportContext context = API.BitmapExportContextService.CreateContext((int)width, (int)height);
            ICanvas? canvas = context.Canvas;

            List<Color> colors = new() { ColorService.Full, ColorService.Background };

            canvas.SetShadow(new SizeF(6, 8), 10f, ColorService.BackgroundHigh);

            RadialGradientPaint paintFrom = new(new Point(1, 1), 1)
            {
                StartColor = colors[0],
                EndColor = colors[1]
            };

            canvas.SetFillPaint(paintFrom, new RectF(0f, 0f, width, height));
            canvas.FillRectangle(0, 0, width, height);

            RadialGradientPaint paintTo = new(new Point(1, 1), 1) { StartColor = ColorService.Foreground, EndColor = colors[0] };

            canvas.SetFillPaint(paintTo, new RectF(0f, 0f, width, height));
            canvas.FontSize = 120;
            canvas.Font = new Font("Maple Mono");
            canvas.FontColor = paintTo.ToColor();
            canvas.DrawString("ACDCs", 0, 0, width, height, HorizontalAlignment.Center, VerticalAlignment.Center);

            MemoryStream ms = new();

            context.Image.Save(ms);
            ms.Position = 0;

            ImageSource? source = ImageSource.FromStream(() => ms);
            return source;
        }
        catch
        {
            // ignored
        }

        return null;
    }

    public static ImageSource? ButtonImageSource(string text, int width, int height)
    {
        try
        {
            using BitmapExportContext context = API.BitmapExportContextService.CreateContext(width, height);
            ICanvas? canvas = context.Canvas;
            canvas.Alpha = 0.7f;

            List<Color> colors = new() { ColorService.Background, ColorService.BackgroundHigh };

            canvas.SetShadow(new SizeF(2, 4), 10f, colors[1]);

            LinearGradientPaint paintFrom = new(Point.Zero, new Point(1, 1))
            {
                StartColor = colors[0],
                EndColor = colors[1]
            };
            canvas.SetFillPaint(paintFrom, new RectF(0f, 0f, width, height));
            canvas.FillRoundedRectangle(0, 0, width, height, 2);
            canvas.StrokeSize = 3;
            canvas.StrokeColor = ColorService.Border;
            canvas.Antialias = false;
            canvas.DrawRoundedRectangle(2, 2, width - 4, height - 4, 2);
            canvas.Antialias = true;

            canvas.FontColor = ColorService.Text;
            canvas.Font = new Font("Maple Mono");
            canvas.DrawString(text, width / 2, height / 2, HorizontalAlignment.Center);

            MemoryStream ms = new();

            context.Image.Save(ms);
            ms.Position = 0;

            ImageSource? source = ImageSource.FromStream(() => ms);
            return source;
        }
        catch
        {
            // ignored
        }

        return null;
    }

    public static ImageSource? WindowImageSource(float width, float height)
    {
        try
        {
            using BitmapExportContext context = API.BitmapExportContextService.CreateContext((int)width, (int)height);
            ICanvas? canvas = context.Canvas;
            canvas.Alpha = 0.7f;

            List<Color> colors = new() { ColorService.Background, ColorService.Foreground };

            RadialGradientPaint paintFrom = new(new Point(1, 0), 1)
            {
                StartColor = colors[0],
                EndColor = colors[1]
            };

            canvas.SetFillPaint(paintFrom, new RectF(0f, 0f, width, height));
            canvas.FillRoundedRectangle(0, 0, width, height, 1);
            canvas.StrokeSize = 1;
            canvas.StrokeColor = ColorService.Border;
            canvas.Antialias = false;
            canvas.DrawRoundedRectangle(2, 2, width - 4, height - 4, 1);

            canvas.Antialias = true;
            canvas.FillRoundedRectangle(0, 0, width, 34, 1);
            canvas.Antialias = false;
            canvas.DrawRoundedRectangle(2, 2, width - 4, 36, 1);

            MemoryStream ms = new();

            context.Image.Save(ms);
            ms.Position = 0;

            ImageSource? source = ImageSource.FromStream(() => ms);
            return source;
        }
        catch
        {
            // ignored
        }

        return null;
    }
}
