using System.Collections.Concurrent;
using Microsoft.AppCenter.Crashes;
using Microsoft.Maui.Graphics.Skia;

namespace ACDCs;

public delegate void ResetEvent(object sender, ResetEventArgs args);

public class ResetEventArgs
{
}

public static class API
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object?>> s_comValues = new();
    public static PlatformBitmapExportService BitmapExportContextService { get; set; } = new();
    public static Page? MainPage { get; set; }
    public static Action<Point>? PointerCallback { get; set; }
    public static Element? PointerLayoutObjectToMeasure { get; set; }

    public static event ResetEvent? Reset;

    public static async Task Call(Func<Task> action, bool disableReset = false)
    {
        if (!disableReset)
        {
            try
            {
                OnReset(new ResetEventArgs());
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }

        try
        {
            await action().ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            Crashes.TrackError(exception);

            await PopupException(exception);
        }
    }

    public static T? Com<T>(string name, string property, object? value = null)
    {
        if (value != null)
        {
            if (!s_comValues.ContainsKey(name))
                s_comValues.GetOrAdd(name, new ConcurrentDictionary<string, object?>());
            if (!s_comValues[name].ContainsKey(property))
            {
                s_comValues[name].GetOrAdd(property, value);
            }
            else
            {
                s_comValues[name][property] = value;
            }
        }

        if (s_comValues.ContainsKey(name) && s_comValues[name].ContainsKey(property))
        {
            return (T)s_comValues[name][property]!;
        }

        return default;
    }

    public static async Task<string> LoadMauiAssetAsString(string name)
    {
        await using Stream stream = await FileSystem.OpenAppPackageFileAsync(name);
        using StreamReader reader = new(stream);

        string contents = await reader.ReadToEndAsync();
        return contents;
    }

    public static async Task PopupException(Exception exception)
    {
        if (MainPage != null)
        {
            await MainPage.DisplayAlert("Internal exception", exception.Message, "ok");
        }
    }

    private static void OnReset(ResetEventArgs args)
    {
        Reset?.Invoke(new object(), args);
    }
}
