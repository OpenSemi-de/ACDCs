namespace ACDCs.API.Instance;

using System.Collections.Concurrent;
using Interfaces;
using IO.DB;
using Microsoft.AppCenter.Crashes;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using Shared.Delegates;
using Sharp.UI;
using AppTheme = AppTheme;
using Color = Color;

public class API : IWorkbenchService, IImageService, IColorService, IDescriptionService, IEditService, IMenuService, IFileService, IImportService
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object>> s_comValues = new();
    private readonly IColorService _colorService;
    private readonly IDescriptionService _descriptionService;
    private readonly IEditService _editService;
    private readonly IFileService _fileService;
    private readonly IImageService _imageService;
    private readonly IImportService _importService;
    private readonly IMenuService _menuService;
    private readonly IWorkbenchService _workbenchService;
    public static PlatformBitmapExportService BitmapExportContextService { get; } = new();
    public static API Instance { get; private set; } = null!;
    public static IWindowContainer? MainContainer { get; set; }
    public static Page MainPage { get; set; } = null!;

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public static Action<Point>? PointerCallback { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public static Element? PointerLayoutObjectToMeasure { get; set; }

    public static ResourceDictionary? Resources { get; set; }
    public static IWindowTabBar? TabBar { get; set; }
    public static Action<Exception, IDictionary<string, string>?, ErrorAttachmentLog[]?>? TrackError { get; set; }
    public static AppTheme UserAppTheme { get; set; }

    public Color Background => _colorService.Background;
    public Color BackgroundHigh => _colorService.BackgroundHigh;
    public Color Border => _colorService.Border;

    public Color Foreground => _colorService.Foreground;
    public Color Full => _colorService.Full;
    public Color Text => _colorService.Text;

    private static PreferencesRepository PreferencesRepository { get; } = new();

    public API(IWorkbenchService workbenchServiceImplementation, IImageService imageServiceImplementation,
        IColorService colorService, IDescriptionService descriptionService, IEditService editService, IMenuService menuService, IFileService fileService, IImportService importService)
    {
        _workbenchService = workbenchServiceImplementation;
        _imageService = imageServiceImplementation;
        _colorService = colorService;
        _descriptionService = descriptionService;
        _editService = editService;
        _menuService = menuService;
        _fileService = fileService;
        _importService = importService;
        Instance = this;
    }

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
                TrackError?.Invoke(exception, null, null);
            }
        }

        try
        {
            await action().ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            //TrackError?.Invoke(exception, null, null);

            await PopupException(exception);
        }
    }

    public static T? Com<T>(string name, string property, object? value = null)
    {
        if (value != null)
        {
            if (!s_comValues.ContainsKey(name))
                s_comValues.GetOrAdd(name, new ConcurrentDictionary<string, object>());
            if (!s_comValues[name].ContainsKey(property))
            {
                s_comValues[name].GetOrAdd(property, value);
            }
            else
            {
                s_comValues[name][property] = value;
            }
        }

        if (s_comValues.TryGetValue(name, out ConcurrentDictionary<string, object>? newValue) && newValue.ContainsKey(property))
        {
            return (T)s_comValues[name][property];
        }

        return default;
    }

    public static object GetPreference(string key)
    {
        return PreferencesRepository.GetPreference(key)!;
    }

    public static T? GetPreference<T>(string v) where T : class
    {
        return GetPreference(v) as T;
    }

    public static async Task<string> LoadMauiAssetAsString(string? name)
    {
        if (name == null)
        {
            return string.Empty;
        }

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

    public static void SetPreference(string name, object preference)
    {
        PreferencesRepository.SetPreference(name, preference);
    }

    public void Add(string name, Action<object?> action) => _menuService.Add(name, action);

    public ImageSource? BackgroundImageSource(ContentPage view) => _imageService.BackgroundImageSource(view);

    public ImageSource? BackgroundImageSource(float width, float height) => _imageService.BackgroundImageSource(width, height);

    public ImageSource? BackgroundImageSource(View view) => _imageService.BackgroundImageSource(view);

    public ImageSource? ButtonImageSource(string text, int width, int height, string font) => _imageService.ButtonImageSource(text, width, height, font);

    public void Call(string menuCommand) => _menuService.Call(menuCommand);

    public void Call(string menuCommand, object param) => _menuService.Call(menuCommand, param);

    public Task Delete(ICircuitView circuitView) => _editService.Delete(circuitView);

    public Task DeselectAll(ICircuitView circuitView) => _editService.DeselectAll(circuitView);

    public Task Duplicate(ICircuitView circuitView) => _editService.Duplicate(circuitView);

    public Color GetColor(string colorName) => _colorService.GetColor(colorName);

    public string GetComponentDescription(Type parentType, string propertyName) => _descriptionService.GetComponentDescription(parentType, propertyName);

    public int GetComponentPropertyOrder(Type parentType, string propertyName) => _descriptionService.GetComponentPropertyOrder(parentType, propertyName);

    public Page GetWorkbenchPage() => _workbenchService.GetWorkbenchPage();

    public Task ImportSpiceModels(IComponentsView componentsView) => _importService.ImportSpiceModels(componentsView);

    public Task Mirror(ICircuitView circuitView) => _editService.Mirror(circuitView);

    public Task NewFile(ICircuitView circuitView) => _fileService.NewFile(circuitView);

    public Task OpenDB(IComponentsView componentsView) => _importService.OpenDB(componentsView);

    public Task OpenFile(ICircuitView circuitView) => _fileService.OpenFile(circuitView);

    public Task Rotate(ICircuitView circuitView) => _editService.Rotate(circuitView);

    public Task SaveFile(ICircuitView circuitView, Page popupPage) => _fileService.SaveFile(circuitView, popupPage);

    public Task SaveFileAs(Page popupPage, ICircuitView circuitView) => _fileService.SaveFileAs(popupPage, circuitView);

    public void SaveJson() => _importService.SaveJson();

    public void SaveToDB(IComponentsView componentsView) => _importService.SaveToDB(componentsView);

    public Task SelectAll(ICircuitView circuitView) => _editService.SelectAll(circuitView);

    public Task ShowProperties(ICircuitView circuitView) => _editService.ShowProperties(circuitView);

    public Task SwitchMultiselect(object? state, ICircuitView circuitView) => _editService.SwitchMultiselect(state, circuitView);

    public ImageSource? WindowImageSource(float width, float height, int titleHeight) => _imageService.WindowImageSource(width, height, titleHeight);

    private static void OnReset(ResetEventArgs args)
    {
        Reset?.Invoke(new object(), args);
    }
}
