namespace ACDCs.API.Core.Components.Preferences;

using ACDCs.API.Windowing.Components.Window;

// ReSharper disable once UnusedType.Global
public class PreferencesWindow : Window
{
    private static PreferencesWindow? s_instance;

    // ReSharper disable once ConvertToAutoPropertyWhenPossible
    // ReSharper disable once UnusedMember.Global
    public static PreferencesWindow? Instance
    {
        get => s_instance;
        set => s_instance = value;
    }

    public PreferencesWindow(WindowContainer? layout) : base(layout, "Preferences", childViewFunction: GetView)
    {
        s_instance = this;

        OnClose = () =>
        {
            s_instance = null;
            return false;
        };
        Start();
    }

    private static View GetView(Window window)
    {
        return new PreferencesView();
    }
}
