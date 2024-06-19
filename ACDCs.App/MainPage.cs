namespace ACDCs.App;

using ACDCs.Interfaces;

/// <summary>
/// The MainPage to start the app.
/// </summary>
/// <seealso cref="Microsoft.Maui.Controls.ContentPage" />
public class MainPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// </summary>
    /// <param name="desktopView">The desktop view.</param>
    public MainPage(IDesktopView desktopView)
    {
        Content = (View)desktopView;
    }
}