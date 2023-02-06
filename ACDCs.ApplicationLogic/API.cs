namespace ACDCs.ApplicationLogic;

using System.Collections.Concurrent;
using ACDCs.ApplicationLogic.Components.Window;
using Components;
using Components.Circuit;
using Interfaces;
using IO.DB;
using Microsoft.AppCenter.Crashes;
using Microsoft.Maui.Graphics.Skia;
using Services;
using Sharp.UI;

public delegate void ResetEvent(object sender, ResetEventArgs args);

public class ResetEventArgs
{
}

public class API : IWorkbenchService, IImageService, IColorService, IDescriptionService, IEditService, IMenuService, IFileService, IImportService
{
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object?>> s_comValues = new();
    private readonly IDescriptionService _descriptionService;
    private readonly IEditService _editService;
    private readonly IFileService _fileService;
    private readonly IImageService _imageService;
    private readonly IImportService _importService;
    private readonly IMenuService _menuService;
    private readonly IWorkbenchService _workbenchService;
    private IColorService _colorService;
    public static PlatformBitmapExportService BitmapExportContextService { get; } = new();
    public static API Instance { get; set; }
    public static WindowContainer? MainContainer { get; set; }
    public static Page? MainPage { get; set; }
    public static Action<Point>? PointerCallback { get; set; }
    public static Element? PointerLayoutObjectToMeasure { get; set; }
    public static ResourceDictionary? Resources { get; set; }
    public static WindowTabBar? TabBar { get; set; }
    public static Action<Exception, IDictionary<string, string>?, ErrorAttachmentLog[]?> TrackError { get; set; }
    public static AppTheme UserAppTheme { get; set; }

    public Color Background => _colorService.Background;
    public Color BackgroundHigh => _colorService.BackgroundHigh;
    public Color Border => _colorService.Border;

    public IColorService ColorService
    {
        get => _colorService;
        set => _colorService = value;
    }

    public Color Foreground => _colorService.Foreground;
    public Color Full => _colorService.Full;
    public Color Text => _colorService.Text;

    private static PreferencesRepository PreferencesRepository { get; } = new();

    private API(IWorkbenchService workbenchServiceImplementation, IImageService imageServiceImplementation,
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

        _imageService.ColorService = _colorService;
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
                TrackError(exception, null, null);
            }
        }

        try
        {
            await action().ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            TrackError(exception, null, null);

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

    public static API GetAPIInstance()
    {
        return new API(
            new WorkbenchService(),
            new ImageService(),
            new ColorService(Resources, UserAppTheme),
            new DescriptionService(),
            new EditService(),
            new MenuService(),
            new FileService(),
            new ImportService()
        );
    }

    public static object GetPreference(string key)
    {
        return PreferencesRepository.GetPreference(key)!;
    }

    public static async Task<string> LoadMauiAssetAsString(string? name)
    {
        await using Stream stream = await FileSystem.OpenAppPackageFileAsync(name);
        using StreamReader reader = new(stream);

        string contents = await reader.ReadToEndAsync();
        return contents;
    }

    public static async Task Open(Window window, WindowState windowState = WindowState.Maximized)
    {
        await Call(() =>
        {
            if (windowState == WindowState.Maximized)
                window.Maximize();
            TabBar?.AddWindow(window);
            return Task.CompletedTask;
        });
    }

    public static async Task PopupException(Exception exception)
    {
        if (MainPage != null)
        {
            await MainPage.DisplayAlert("Internal exception", exception.Message, "ok");
        }
    }

    public void Add(string name, Action<object?> action) => _menuService.Add(name, action);

    public ImageSource? BackgroundImageSource(ContentPage view) => _imageService.BackgroundImageSource(view);

    public ImageSource? BackgroundImageSource(float width, float height) => _imageService.BackgroundImageSource(width, height);

    public ImageSource? BackgroundImageSource(View view) => _imageService.BackgroundImageSource(view);

    public ImageSource? ButtonImageSource(string text, int width, int height) => _imageService.ButtonImageSource(text, width, height);

    public void Call(string menuCommand) => _menuService.Call(menuCommand);

    public void Call(string menuCommand, object param) => _menuService.Call(menuCommand, param);

    public Task Delete(CircuitView circuitView) => _editService.Delete(circuitView);

    public Task DeselectAll(CircuitView circuitView) => _editService.DeselectAll(circuitView);

    public Task Duplicate(CircuitView circuitView) => _editService.Duplicate(circuitView);

    public Color GetColor(string colorName) => ColorService.GetColor(colorName);

    public string GetComponentDescription(Type parentType, string propertyName) => _descriptionService.GetComponentDescription(parentType, propertyName);

    public int GetComponentPropertyOrder(Type parentType, string propertyName) => _descriptionService.GetComponentPropertyOrder(parentType, propertyName);

    public Page GetWorkbenchPage() => _workbenchService.GetWorkbenchPage();

    public Task ImportSpiceModels(ComponentsView componentsView) => _importService.ImportSpiceModels(componentsView);

    public Task Mirror(CircuitView circuitView) => _editService.Mirror(circuitView);

    public Task NewFile(CircuitView circuitView) => _fileService.NewFile(circuitView);

    public Task OpenDB(ComponentsView componentsView) => _importService.OpenDB(componentsView);

    public Task OpenFile(CircuitView circuitView) => _fileService.OpenFile(circuitView);

    public Task Rotate(CircuitView circuitView) => _editService.Rotate(circuitView);

    public Task SaveFile(CircuitView circuitView, Page popupPage) => _fileService.SaveFile(circuitView, popupPage);

    public Task SaveFileAs(Page popupPage, CircuitView circuitView) => _fileService.SaveFileAs(popupPage, circuitView);

    public void SaveJson() => _importService.SaveJson();

    public void SaveToDB(ComponentsView componentsView) => _importService.SaveToDB(componentsView);

    public Task SelectAll(CircuitView circuitView) => _editService.SelectAll(circuitView);

    public Task SwitchMultiselect(object? state, CircuitView circuitView) => _editService.SwitchMultiselect(state, circuitView);

    public ImageSource? WindowImageSource(float width, float height) => _imageService.WindowImageSource(width, height);

    private static void OnReset(ResetEventArgs args)
    {
        Reset?.Invoke(new object(), args);
    }
}
