using ACDCs.Views;

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
            using BitmapExportContext context = App.BitmapExportContextService.CreateContext((int)width, (int)height, 1f);
            List<Color> colors;
            ICanvas? canvas = context.Canvas;

            colors = new List<Color> { ColorManager.Background, ColorManager.BackgroundHigh };

            canvas.SetShadow(new SizeF(2, 4), 10f, colors[1]);

            LinearGradientPaint paintFrom = new(Point.Zero, new(1, 1))
            {
                StartColor = colors[0],
                EndColor = colors[1],
            };
            canvas.SetFillPaint(paintFrom, new RectF(0f, 0f, width, height));
            canvas.FillRectangle(0, 0, width, height);

            canvas.Alpha = 0.7f;
            LinearGradientPaint paintTo = new(Point.Zero, new(1, 1)) { StartColor = colors[1], EndColor = colors[0], };

            canvas.SetFillPaint(paintTo, new RectF(0f, 0f, width, height));

            PathF path = new();

            path.MoveTo(0, 0)
                .LineTo(width, 0)
                .LineTo(width, 10)
                .LineTo(30, 10)
                .CurveTo(10, 10, 10, 10, 10, 30)
                .LineTo(10, height)
                .LineTo(0, height)
                .LineTo(0, 0);

            canvas.FillPath(path, WindingMode.EvenOdd);

            MemoryStream ms = new();

            context.Image.Save(ms);
            ms.Position = 0;

            ImageSource? source = ImageSource.FromStream(() => ms);
            return source;
        }
        catch (Exception e)
        {
        }
        return null;
    }

    public static ImageSource? ButtonImageSource(string text, int width, int height)
    {
        try
        {
            using BitmapExportContext context = App.BitmapExportContextService.CreateContext((int)width, (int)height, 1f);
            List<Color> colors;
            ICanvas? canvas = context.Canvas;
            canvas.Alpha = 0.7f;

            colors = new List<Color> { ColorManager.Background, ColorManager.BackgroundHigh };

            canvas.SetShadow(new SizeF(2, 4), 10f, colors[1]);

            LinearGradientPaint paintFrom = new(Point.Zero, new(1, 1))
            {
                StartColor = colors[0],
                EndColor = colors[1],
            };
            canvas.SetFillPaint(paintFrom, new RectF(0f, 0f, width, height));
            canvas.FillRectangle(0, 0, width, height);
            PathF path = new();

            path.MoveTo(0, 0)
                .LineTo(width, 0)
                .LineTo(width, 3)
                .LineTo(9, 3)
                .CurveTo(3, 3, 3, 3, 3, 9)
                .LineTo(3, height)
                .LineTo(0, height)
                .LineTo(0, 0);

            LinearGradientPaint paintTo = new(Point.Zero, new(1, 1))
            {
                StartColor = colors[1],
                EndColor = colors[0],
            };

            canvas.SetFillPaint(paintTo, new RectF(0f, 0f, width, height));

            canvas.FillPath(path, WindingMode.EvenOdd);
            canvas.FontColor = colors[2];
            canvas.DrawString(text, width / 2, height / 2, HorizontalAlignment.Center);

            MemoryStream ms = new();

            context.Image.Save(ms);
            ms.Position = 0;

            ImageSource? source = ImageSource.FromStream(() => ms);
            return source;
        }
        catch (Exception e)
        {
        }

        return null;
    }
}
