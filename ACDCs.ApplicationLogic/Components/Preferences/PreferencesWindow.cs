namespace ACDCs.ApplicationLogic.Components.Preferences;

using Window;

public class PreferencesWindow : Window
{
    private static PreferencesWindow? s_instance;

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
