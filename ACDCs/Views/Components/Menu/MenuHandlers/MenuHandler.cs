namespace ACDCs.Views.Components.Menu.MenuHandlers;

public class MenuHandler
{
    private static readonly Dictionary<string, object> s_menuHandlers = new();

    public static void Add(string name, object action)
    {
        API.Call(() =>
        {
            if (!s_menuHandlers.ContainsKey(name))
            {
                s_menuHandlers.Add(name, action);
            }
            else
            {
                s_menuHandlers[name] = action;
                GC.Collect();
            }

            return Task.CompletedTask;
        }).Wait();
    }

    public static void Call(string menuCommand)
    {
        API.Call(() =>
        {
            if (s_menuHandlers.ContainsKey(menuCommand))
            {
                ((Action)s_menuHandlers[menuCommand]).Invoke();
            }

            return Task.CompletedTask;
        }).Wait();
    }

    public static void Call(string menuCommand, object param)
    {
        API.Call(() =>
        {
            if (s_menuHandlers.ContainsKey(menuCommand))
            {
                ((Action<object>)s_menuHandlers[menuCommand]).Invoke(param);
            }

            return Task.CompletedTask;
        }).Wait();
    }
}