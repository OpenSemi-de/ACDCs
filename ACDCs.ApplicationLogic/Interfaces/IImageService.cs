using ContentPage = Sharp.UI.ContentPage;

namespace ACDCs.ApplicationLogic.Interfaces;

public interface IImageService
{
    IColorService ColorService { get; set; }

    ImageSource? BackgroundImageSource(ContentPage view);

    ImageSource? BackgroundImageSource(float width, float height);

    ImageSource? BackgroundImageSource(View view);

    ImageSource? ButtonImageSource(string text, int width, int height);

    ImageSource? WindowImageSource(float width, float height);
}
