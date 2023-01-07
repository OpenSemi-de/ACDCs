using System.Collections.Concurrent;
using ACDCs.Views;
using Microsoft.AppCenter.Crashes;
using Microsoft.Maui.Graphics.Skia;

namespace ACDCs;

public partial class App : Application
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object?>> _comValues = new();

    public static PlatformBitmapExportService BitmapExportContextService { get; set; } = new();

    public App()
    {
        InitializeComponent();
        UserAppTheme = AppTheme.Dark;
        MainPage = new StartCenterPage();
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
    }

    public static event ResetEvent? Reset;

    public static async Task Call(Func<Task> action, bool disableReset = false)
    {
        if (!disableReset)
        {
            try
            {
                OnReset(new());
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

            await Shell.Current.CurrentPage.DisplayAlert("Internal exception", exception.Message.ToString(), "ok");
        }
    }

    public static T? Com<T>(string name, string property, object? value = null)
    {
        if (value != null)
        {
            if (!_comValues.ContainsKey(name))
                _comValues.GetOrAdd(name, new ConcurrentDictionary<string, object?>());
            if (!_comValues[name].ContainsKey(property))
            {
                _comValues[name].GetOrAdd(property, value);
            }
            else
            {
                _comValues[name][property] = value;
            }
        }

        if (_comValues.ContainsKey(name) && _comValues[name].ContainsKey(property))
        {
            return (T)_comValues[name][property]!;
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

    private static void OnReset(ResetEventArgs args)
    {
        Reset?.Invoke(new(), args);
    }

    private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Crashes.TrackError(e.ExceptionObject as Exception);
    }
}

public class ResetEventArgs
{
}

public delegate void ResetEvent(object sender, ResetEventArgs args);
