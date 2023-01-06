namespace ACDCs.Views;

public static class ColorManager
{
    private static ResourceDictionary? s_resourceColors;

    public static Color Background => GetColor(nameof(Background));
    public static Color BackgroundHigh => GetColor(nameof(BackgroundHigh));
    public static Color Border => GetColor(nameof(Border));
    public static Color Foreground => GetColor(nameof(Foreground));
    public static Color Text => GetColor(nameof(Text));

    private static Color GetColor(string colorName)
    {
        Color color = Colors.Transparent;
        if (colorName != "")
        {
            s_resourceColors ??= App.Current?.Resources.MergedDictionaries.First(dic => dic.Source.ToString().Contains("Colors.xaml"));
            string theme = App.Current?.UserAppTheme == AppTheme.Dark ? "Dark" : "Light";
            if (s_resourceColors != null)
            {
                if (s_resourceColors.TryGetValue(colorName + theme, out object value))
                {
                    color = value as Color ?? Colors.Purple;
                }
            }
        }

        return color;
    }
}
