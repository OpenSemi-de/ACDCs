using ACDCs.ApplicationLogic.Interfaces;

namespace ACDCs.ApplicationLogic.Services;

public class ColorService : IColorService
{
    private ResourceDictionary? _resourceColors;

    public Color Background => GetColor(nameof(Background));
    public Color BackgroundHigh => GetColor(nameof(BackgroundHigh));
    public Color Border => GetColor(nameof(Border));
    public Color Foreground => GetColor(nameof(Foreground));
    public Color Full => GetColor(nameof(Full));
    public ResourceDictionary? Resources { get; set; }
    public Color Text => GetColor(nameof(Text));

    public AppTheme UserAppTheme { get; set; }

    public ColorService(ResourceDictionary? resources, AppTheme userAppTheme)
    {
        Resources = resources;
        UserAppTheme = userAppTheme;
    }

    public Color GetColor(string colorName)
    {
        Color color = Colors.Transparent;
        if (colorName == "")
        {
            return color;
        }

        _resourceColors ??= Resources?.MergedDictionaries.First(dic => dic.Source.ToString().Contains("Colors.xaml"));
        if (_resourceColors == null)
        {
            return color;
        }

        string theme = UserAppTheme == AppTheme.Dark ? "Dark" : "Light";
        if (_resourceColors.TryGetValue(colorName + theme, out object value))
        {
            color = value as Color ?? Colors.Purple;
        }

        return color;
    }
}
