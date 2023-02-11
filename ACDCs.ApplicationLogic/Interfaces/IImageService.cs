namespace ACDCs.ApplicationLogic.Interfaces;

using Sharp.UI;

public interface IImageService
{
    ImageSource? BackgroundImageSource(ContentPage view);

    ImageSource? BackgroundImageSource(float width, float height);

    ImageSource? BackgroundImageSource(View view);

    ImageSource? ButtonImageSource(string text, int width, int height);

    ImageSource? WindowImageSource(float width, float height);
}
