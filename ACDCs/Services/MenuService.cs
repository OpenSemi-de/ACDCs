namespace ACDCs.Services;

public static class MenuService
{
    private static readonly Dictionary<string, Action<object?>> s_menuHandlers = new();

    public static void Add(string name, Action<object?> action)
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
                s_menuHandlers[menuCommand].Invoke(null);
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
                s_menuHandlers[menuCommand].Invoke(param);
            }

            return Task.CompletedTask;
        }).Wait();
    }
}
