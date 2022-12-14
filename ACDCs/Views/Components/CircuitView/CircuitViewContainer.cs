using ACDCs.CircuitRenderer;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Scene;
using ACDCs.CircuitRenderer.Sheet;
using Newtonsoft.Json;
using Color = Microsoft.Maui.Graphics.Color;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace ACDCs.Views.Components.CircuitView;

using Sharp.UI;

[BindableProperties]
public interface ICircuitViewProperties
{
    public Color BackgroundHighColor { get; set; }
    public Color ForegroundColor { get; set; }
    public AbsoluteLayout PopupTarget { get; set; }
}

[SharpObject]
public partial class CircuitViewContainer : ContentView, ICircuitViewProperties
{
    private readonly Workbook _currentWorkbook;

    private readonly GraphicsView _graphicsView;

    private readonly PanGestureRecognizer _panRecognizer;

    private readonly PointerGestureRecognizer _pointerRecognizer;

    private readonly TapGestureRecognizer _tapRecognizer;

    private Worksheet _currentSheet;

    private Dictionary<string, string> _cursorDebugValues = new();
    private Point _cursorPosition;

    private PointF _dragStartPosition;

    private bool _isDraggingItem;
    private Coordinate? _lastDisplayOffset;

    private bool _multiSelectionMode;

    private Dictionary<WorksheetItem, Coordinate> _selectedItemsBasePositions = new();

    public Worksheet CurrentWorksheet
    {
        get => _currentSheet;
        set => _currentSheet = value;
    }

    public Action? CursorDebugChanged { get; set; }

    public string CursorDebugOutput { get; set; }

    public Action<WorksheetItem>? OnSelectedItemChange { get; set; }
    public WorksheetItem? SelectedItem { get; set; }

    private Action<WorksheetItemList, WorksheetItemList>? ListSetItems
    {
        get => App.Com<Action<WorksheetItemList, WorksheetItemList>>("ItemList", "SetItems");
    }

    public CircuitViewContainer()
    {
        _multiSelectionMode = false;
        _currentWorkbook = new Workbook();
        _currentWorkbook.SetBaseFont("Maple Mono");

        _currentSheet = _currentWorkbook.AddNewSheet();

        this.BackgroundColor(Colors.Transparent);

        _tapRecognizer = new Microsoft.Maui.Controls.TapGestureRecognizer();
        _tapRecognizer.Tapped += TapGestureRecognizer_OnTapped;

        _panRecognizer = new PanGestureRecognizer();
        _panRecognizer.PanUpdated += PanGestureRecognizer_OnPanUpdated;

        _pointerRecognizer = new PointerGestureRecognizer();
        _pointerRecognizer.PointerMoved += PointerGestureRecognizer_OnPointerMoved;

        _graphicsView = new GraphicsView()
            .BackgroundColor(Colors.Transparent)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .GestureRecognizers(_panRecognizer)
            .GestureRecognizers(_pointerRecognizer)
        .GestureRecognizers(_tapRecognizer);

        Content = _graphicsView;
        App.Com<CircuitViewContainer>(nameof(CircuitView), "Instance", this);
        App.Com<Worksheet>(nameof(CircuitView), "_currentSheet", _currentSheet);
        Loaded += OnLoaded;
    }

    public event CursorPositionChangeEvent? CursorPositionChanged;

    public event EventHandler<EventArgs>? LoadedSheet;

    public event EventHandler<EventArgs>? SavedSheet;

    public event CursorPositionChangeEvent? TapPositionChanged;

    public void Clear()
    {
        _currentWorkbook.Sheets.Clear();
        _currentSheet = _currentWorkbook.AddNewSheet();
    }

    public async Task InsertToPosition(float x, float y, WorksheetItem? newItem)
    {
        await App.Call(() =>
        {
            if (newItem != null)
            {
                newItem.DrawableComponent.Position.X = x;
                newItem.DrawableComponent.Position.Y = y;
                newItem.X -= newItem.Width / 2;
                newItem.Y -= newItem.Height / 2;

                _currentSheet.Items.AddItem(newItem);
            }

            App.Com<bool>("Items", "IsInserting", false);

            ListSetItems?.Invoke(_currentSheet.Items, _currentSheet.SelectedItems);

            return Task.CompletedTask;
        });
    }

    public async Task InsertToPosition(float x, float y)
    {
        await App.Call(() =>
        {
            Func<float, float, Worksheet, WorksheetItem?>? doInsert = App.Com<Func<float, float, Worksheet, WorksheetItem?>?>("Items", "DoInsert");
            WorksheetItem? newItem = doInsert?.Invoke(x, y, _currentSheet);
            if (newItem != null)
            {
                newItem.X -= newItem.Width / 2;
                newItem.Y -= newItem.Height / 2;
            }

            App.Com<bool>("Items", "IsInserting", false);

            ListSetItems?.Invoke(_currentSheet.Items, _currentSheet.SelectedItems);

            return Task.CompletedTask;
        });
    }

    public async void Open(string fileName)
    {
        JsonSerializerSettings settings = new()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            Formatting = Formatting.Indented,
            Error = JsonError,
            TypeNameHandling = TypeNameHandling.Objects
        };

        string jsonData = await File.ReadAllTextAsync(fileName);
        Worksheet? newSheet = JsonConvert.DeserializeObject<Worksheet>(jsonData, settings);
        if (newSheet != null)
        {
            _currentWorkbook.Sheets.Clear();
            _currentWorkbook.Sheets.AddSheet(newSheet);
            _currentSheet = newSheet;

            App.Com<Worksheet>(nameof(CircuitView), "_currentSheet", _currentSheet);
            _currentSheet.Filename = System.IO.Path.GetFileName(fileName);
            await Paint();
        }

        OnLoadedSheet();
    }

    public async Task Paint()
    {
        await App.Call(() =>
        {
            if (BackgroundColor != null)
            {
                _currentSheet.BackgroundColor = new CircuitRenderer.Definitions.Color(BackgroundColor.WithAlpha(0.2f));
            }

            if (ForegroundColor != null)
            {
                _currentSheet.ForegroundColor = new CircuitRenderer.Definitions.Color(ForegroundColor);
            }

            if (BackgroundHighColor != null)
            {
                _currentSheet.BackgroundHighColor = new CircuitRenderer.Definitions.Color(BackgroundHighColor);
            }

            if (App.Com<bool>("Items", "IsInserting"))
                return Task.CompletedTask;

            _graphicsView.Drawable = null;
            _currentSheet?.CalculateScene();
            DrawableScene? scene = (DrawableScene?)
                _currentSheet?.SceneManager?.GetSceneForBackend();
            _graphicsView.Drawable = scene;

            _graphicsView.Invalidate();
            return Task.CompletedTask;
        });
    }

    public async void SaveAs(string fileName)
    {
        JsonSerializerSettings settings = new()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All
        };
        string jsonData = JsonConvert.SerializeObject(_currentSheet, settings: settings);
        _currentSheet.Filename = System.IO.Path.GetFileName(fileName);
        await File.WriteAllTextAsync(fileName, jsonData);
        OnSavedSheet();
    }

    public void UseMultiselect(bool state)
    {
        _currentSheet.UseMultiselect(state);
        Paint().Wait();
    }

    protected virtual void OnCursorPositionChanged(CursorPositionChangeEventArgs args)
    {
        CursorPositionChanged?.Invoke(this, args);
    }

    protected virtual void OnLoadedSheet()
    {
        LoadedSheet?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnSavedSheet()
    {
        SavedSheet?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnTapPositionChanged(CursorPositionChangeEventArgs args)
    {
        TapPositionChanged?.Invoke(this, args);
    }

    private static float GetRelPos(double pos)
    {
        float relPos = 0f;
        App.Call(() =>
        {
            relPos = Convert.ToSingle(Math.Round(pos / (Workbook.BaseGridSize * Workbook.Zoom)));
            return Task.CompletedTask;
        }).Wait();
        return relPos;
    }

    private async Task AddTrace(PinDrawable pinFrom, PinDrawable pinTo)
    {
        await App.Call(async () =>
        {
            _currentSheet.AddRoute(pinFrom, pinTo);
            await Paint();
        });
    }

    private WorksheetItem? GetWorksheetItemaAt(PointF position)
    {
        WorksheetItem? selectedItem = null;
        App.Call(() =>
        {
            float x = GetRelPos(position.X);
            float y = GetRelPos(position.Y);

            IEnumerable<IWorksheetItem> hitItems = _currentSheet.Items.Where(
                item =>
                    x >= item.X && x <= item.X + item.Width &&
                    y >= item.Y && y <= item.Y + item.Height

                                                                            );
            IWorksheetItem[] worksheetItems = hitItems as IWorksheetItem[] ?? hitItems.ToArray();
            if (worksheetItems.Any())
                selectedItem = (WorksheetItem?)worksheetItems.First();
            return Task.CompletedTask;
        }).Wait();

        return selectedItem;
    }

    private void JsonError(object? sender, ErrorEventArgs e)
    {
        Console.WriteLine(e.ErrorContext.Error.ToString());
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        await Paint();
    }

    private void PanGestureRecognizer_OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        SetCursorDebugValue("Pan", e.StatusType + ":" + e.TotalX + "/" + e.TotalY);
        App.Call(async () =>
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                case GestureStatus.Completed:
                    {
                        if (SelectedItem != null)
                            OnSelectedItemChange?.Invoke(SelectedItem);

                        _lastDisplayOffset = _currentSheet?.DisplayOffset ??
                                             new Coordinate(
                                                 Convert.ToSingle(e.TotalX),
                                                 Convert.ToSingle(e.TotalY));
                        if (e.StatusType == GestureStatus.Started)
                        {
                            _selectedItemsBasePositions = new(_currentSheet?.SelectedItems.Select(selectedItem =>
                                new KeyValuePair<WorksheetItem, Coordinate>((WorksheetItem)selectedItem,
                                    new Coordinate(selectedItem.DrawableComponent.Position))) ?? Array.Empty<KeyValuePair<WorksheetItem, Coordinate>>());

                            _dragStartPosition = new PointF(Convert.ToSingle(_cursorPosition.X - _lastDisplayOffset?.X),
                                Convert.ToSingle(_cursorPosition.Y - _lastDisplayOffset?.Y));
                            WorksheetItem? testForItem = GetWorksheetItemaAt(_dragStartPosition);
                            if (testForItem != null)
                            {
                                _isDraggingItem = true;
                            }
                        }

                        if (e.StatusType == GestureStatus.Completed)

                            _isDraggingItem = false;
                        break;
                    }
                case GestureStatus.Running:
                    {
                        if (_currentSheet != null)
                        {
                            if (_isDraggingItem)
                            {
                                PointF cursorPosition = new(Convert.ToSingle(_cursorPosition.X - _lastDisplayOffset?.X),
                                    Convert.ToSingle(_cursorPosition.Y - _lastDisplayOffset?.Y));

                                PointF olddifferenceBetweenCursorPoints = new(_dragStartPosition.X - cursorPosition.X,
                                    _dragStartPosition.Y - cursorPosition.Y);

                                PointF differenceBetweenCursorPoints = new PointF(
                                    Convert.ToSingle(-e.TotalX),
                                    Convert.ToSingle(-e.TotalY));

                                bool changed = false;
                                _currentSheet.SelectedItems.ForEach(item =>
                                    {
                                        if (item != null)
                                        {
                                            Point newPosition = new()
                                            {
                                                X = _selectedItemsBasePositions[(WorksheetItem)item].X,
                                                Y = _selectedItemsBasePositions[(WorksheetItem)item].Y
                                            };

                                            newPosition.X *= Workbook.Zoom * Workbook.BaseGridSize;
                                            newPosition.X -= differenceBetweenCursorPoints.X;
                                            newPosition.X /= Workbook.Zoom * Workbook.BaseGridSize;
                                            newPosition.X -= item.DrawableComponent.Size.X / 2;

                                            newPosition.Y *= Workbook.Zoom * Workbook.BaseGridSize;
                                            newPosition.Y -= differenceBetweenCursorPoints.Y;
                                            newPosition.Y /= Workbook.Zoom * Workbook.BaseGridSize;
                                            newPosition.Y -= item.DrawableComponent.Size.Y / 2;

                                            newPosition.X = Math.Floor(newPosition.X);
                                            newPosition.Y = Math.Floor(newPosition.Y);
                                            if (newPosition.X != item.X ||
                                                newPosition.Y != item.Y)
                                            {
                                                item.X = Convert.ToInt32(newPosition.X);
                                                item.Y = Convert.ToInt32(newPosition.Y);
                                                changed = true;
                                            }
                                        }
                                    });

                                if (changed)
                                    CurrentWorksheet.StartRouter();
                            }
                            else
                            {
                                _currentSheet.DisplayOffset =
                                    new Coordinate(
                                        Convert.ToSingle(e.TotalX),
                                        Convert.ToSingle(e.TotalY)).Add(_lastDisplayOffset ?? new Coordinate());
                            }
                        }

                        await Paint();
                        break;
                    }
            }
        }).Wait();
    }

    private void PointerGestureRecognizer_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        // _cursorPosition = e.GetPosition(_graphicsView) ?? new Point();
        SetCursorDebugValue("Pointer", _cursorPosition.X + "/" + _cursorPosition.Y);

        OnCursorPositionChanged(new CursorPositionChangeEventArgs(_cursorPosition));
    }

    private void SetCursorDebugValue(string key, string value)
    {
        if (!_cursorDebugValues.ContainsKey(key))
            _cursorDebugValues.Add(key, value);
        else
            _cursorDebugValues[key] = value;

        CursorDebugOutput = String.Join(Environment.NewLine, _cursorDebugValues.Select(kv => kv.Key + "=" + kv.Value));
        CursorDebugChanged?.Invoke();
    }

    private async void TapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        Point touch = e.GetPosition(_graphicsView) ?? Point.Zero;
        SetCursorDebugValue("Tap", touch.X + "/" + touch.Y);
        _cursorPosition = touch;
        await App.Call(async () =>
        {
            if (touch != Point.Zero)
            {
                float offsetX = 0;
                float offsetY = 0;
                if (_lastDisplayOffset != null)
                {
                    offsetX = _lastDisplayOffset.X;
                    offsetY = _lastDisplayOffset.Y;
                }

                OnTapPositionChanged(new CursorPositionChangeEventArgs(touch));

                if (App.Com<bool>("Items", "IsInserting"))
                {
                    float x = GetRelPos(touch.X - offsetX);
                    float y = GetRelPos(touch.Y - offsetY);
                    await InsertToPosition(x, y);
                    await Paint();
                }
                else
                {
                    touch.X -= offsetX;
                    touch.Y -= offsetY;

                    WorksheetItem? selectedItem = GetWorksheetItemaAt(touch);

                    if (selectedItem != null)
                    {
                        SelectedItem = selectedItem;
                        OnSelectedItemChange?.Invoke(selectedItem);
                        if (_currentSheet.IsSelected(selectedItem))
                        {
                            float x = GetRelPos(touch.X);
                            float y = GetRelPos(touch.Y);

                            PinDrawable? selectedPin = null;
                            foreach (PinDrawable pin in selectedItem.Pins)
                            {
                                double pinX = Math.Floor(pin.Position.X * selectedItem.Width);
                                double pinY = Math.Floor(pin.Position.Y * selectedItem.Height);
                                pinX += selectedItem.X;
                                pinY += selectedItem.Y;
                                if (pinX == x && pinY == y)
                                {
                                    if (_currentSheet.SelectedPin == pin)
                                    {
                                        selectedPin = null;
                                        _currentSheet.SelectedPin = null;
                                        await Paint();
                                        return;
                                    }
                                    else
                                    {
                                        if (_currentSheet.SelectedPin != null)
                                        {
                                            await AddTrace(pin, _currentSheet.SelectedPin);
                                        }

                                        selectedPin = pin;
                                        _currentSheet.SelectedPin = selectedPin;
                                    }

                                    break;
                                }
                            }

                            if (selectedPin == null)
                            {
                                _currentSheet.ToggleSelectItem(selectedItem);
                            }
                        }
                        else
                        {
                            _currentSheet.ToggleSelectItem(selectedItem);
                        }

                        await Paint();
                    }
                }
            }
        });
    }
}
