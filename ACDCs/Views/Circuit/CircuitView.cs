﻿using ACDCs.CircuitRenderer;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Scene;
using ACDCs.CircuitRenderer.Sheet;
using ACDCs.Components.Circuit;
using ACDCs.Interfaces;
using Newtonsoft.Json;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

using ItemsView = ACDCs.Views.Items.ItemsView;

namespace ACDCs.Views.Circuit;

using Sharp.UI;

[SharpObject]
public partial class CircuitView : ContentView, ICircuitViewProperties
{
    private readonly Workbook _currentWorkbook;
    private readonly Dictionary<string, string> _cursorDebugValues = new();
    private readonly GraphicsView _graphicsView;
    private readonly PanGestureRecognizer _panRecognizer;
    private readonly PointerGestureRecognizer _pointerRecognizer;
    private readonly TapGestureRecognizer _tapRecognizer;
    private Point _cursorPosition;
    private PointF _dragStartPosition;
    private bool _isDraggingItem;
    private Coordinate? _lastDisplayOffset;
    private Dictionary<WorksheetItem, Coordinate> _selectedItemsBasePositions = new();

    public Worksheet CurrentWorksheet { get; set; }

    public Action? CursorDebugChanged { get; set; }
    public string? CursorDebugOutput { get; set; }
    public ItemsView? ItemsView { get; set; }
    public bool MultiSelectionMode { get; set; }
    public Action<WorksheetItem>? OnSelectedItemChange { get; set; }
    public WorksheetItem? SelectedItem { get; set; }

    public CircuitView()
    {
        MultiSelectionMode = false;
        _currentWorkbook = new Workbook();
        _currentWorkbook.SetBaseFont("Maple Mono");

        CurrentWorksheet = _currentWorkbook.AddNewSheet();

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
        API.Com<CircuitView>(nameof(CircuitView), "Instance", this);
        API.Com<Worksheet>(nameof(CircuitView), "_currentSheet", CurrentWorksheet);
        Loaded += OnLoaded;
    }

    public event CursorPositionChangeEvent? CursorPositionChanged;

    public event EventHandler<EventArgs>? LoadedSheet;

    public event EventHandler<EventArgs>? SavedSheet;

    public event CursorPositionChangeEvent? TapPositionChanged;

    public void Clear()
    {
        _currentWorkbook.Sheets.Clear();
        CurrentWorksheet = _currentWorkbook.AddNewSheet();
    }

    public async Task InsertToPosition(float x, float y)
    {
        await API.Call(() =>
        {
            Func<float, float, Worksheet, WorksheetItem?>? doInsert = API.Com<Func<float, float, Worksheet, WorksheetItem?>?>("Items", "DoInsert");
            WorksheetItem? newItem = doInsert?.Invoke(x, y, CurrentWorksheet);
            if (newItem != null)
            {
                newItem.X -= newItem.Width / 2;
                newItem.Y -= newItem.Height / 2;
            }

            if (ItemsView != null)
            {
                ItemsView.IsInserting = false;
            }

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
            CurrentWorksheet = newSheet;

            API.Com<Worksheet>(nameof(CircuitView), "_currentSheet", CurrentWorksheet);
            CurrentWorksheet.Filename = System.IO.Path.GetFileName(fileName);
            await Paint();
        }

        OnLoadedSheet();
    }

    public async Task Paint()
    {
        await API.Call(() =>
        {
            if (BackgroundColor != null)
            {
                CurrentWorksheet.BackgroundColor = new CircuitRenderer.Definitions.Color(BackgroundColor.WithAlpha(0.2f));
            }

            if (ForegroundColor != null)
            {
                CurrentWorksheet.ForegroundColor = new CircuitRenderer.Definitions.Color(ForegroundColor);
            }

            if (BackgroundHighColor != null)
            {
                CurrentWorksheet.BackgroundHighColor = new CircuitRenderer.Definitions.Color(BackgroundHighColor);
            }

            if (ItemsView != null && ItemsView.IsInserting)
                return Task.CompletedTask;

            _graphicsView.Drawable = null;
            CurrentWorksheet.CalculateScene();
            DrawableScene? scene = (DrawableScene?)
                CurrentWorksheet.SceneManager?.GetSceneForBackend();
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
        string jsonData = JsonConvert.SerializeObject(CurrentWorksheet, settings: settings);
        CurrentWorksheet.Filename = System.IO.Path.GetFileName(fileName);
        await File.WriteAllTextAsync(fileName, jsonData);
        OnSavedSheet();
    }

    public void UseMultiselect(bool state)
    {
        CurrentWorksheet.UseMultiselect(state);
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
        API.Call(() =>
        {
            relPos = Convert.ToSingle(Math.Round(pos / (Workbook.BaseGridSize * Workbook.Zoom)));
            return Task.CompletedTask;
        }).Wait();
        return relPos;
    }

    private static void JsonError(object? sender, ErrorEventArgs e)
    {
        Console.WriteLine(e.ErrorContext.Error.ToString());
    }

    private async Task AddTrace(PinDrawable pinFrom, PinDrawable pinTo)
    {
        await API.Call(async () =>
        {
            CurrentWorksheet.AddRoute(pinFrom, pinTo);
            await Paint();
        });
    }

    private WorksheetItem? GetWorksheetItemaAt(PointF position)
    {
        WorksheetItem? selectedItem = null;
        API.Call(() =>
        {
            float x = GetRelPos(position.X);
            float y = GetRelPos(position.Y);

            IEnumerable<IWorksheetItem> hitItems = CurrentWorksheet.Items.Where(
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

    private async void OnLoaded(object? sender, EventArgs e)
    {
        await Paint();
    }

    private void PanGestureRecognizer_OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        SetCursorDebugValue("Pan", e.StatusType + ":" + e.TotalX + "/" + e.TotalY);
        API.Call(async () =>
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                case GestureStatus.Completed:
                    {
                        if (SelectedItem != null)
                            OnSelectedItemChange?.Invoke(SelectedItem);

                        _lastDisplayOffset = CurrentWorksheet.DisplayOffset ??
                                             new Coordinate(
                                                 Convert.ToSingle(e.TotalX),
                                                 Convert.ToSingle(e.TotalY));
                        switch (e.StatusType)
                        {
                            case GestureStatus.Started:
                                {
                                    _selectedItemsBasePositions = new Dictionary<WorksheetItem, Coordinate>(CurrentWorksheet.SelectedItems.Select(selectedItem =>
                                        new KeyValuePair<WorksheetItem, Coordinate>((WorksheetItem)selectedItem,
                                            new Coordinate(selectedItem.DrawableComponent.Position))));

                                    _dragStartPosition = new PointF(Convert.ToSingle(_cursorPosition.X - _lastDisplayOffset?.X),
                                        Convert.ToSingle(_cursorPosition.Y - _lastDisplayOffset?.Y));
                                    WorksheetItem? testForItem = GetWorksheetItemaAt(_dragStartPosition);
                                    if (testForItem != null)
                                    {
                                        _isDraggingItem = true;
                                    }

                                    break;
                                }
                            case GestureStatus.Completed:
                                _isDraggingItem = false;
                                break;
                        }

                        break;
                    }
                case GestureStatus.Running:
                    {
                        if (_isDraggingItem)
                        {
                            PointF differenceBetweenCursorPoints = new(
                                Convert.ToSingle(-e.TotalX),
                                Convert.ToSingle(-e.TotalY));

                            bool changed = false;
                            CurrentWorksheet.SelectedItems.ForEach(item =>
                            {
                                if (item == null)
                                {
                                    return;
                                }

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
                                if (newPosition.X == item.X &&
                                    newPosition.Y == item.Y)
                                {
                                    return;
                                }

                                item.X = Convert.ToInt32(newPosition.X);
                                item.Y = Convert.ToInt32(newPosition.Y);
                                changed = true;
                            });

                            if (changed)
                                CurrentWorksheet.StartRouter();
                        }
                        else
                        {
                            CurrentWorksheet.DisplayOffset =
                                new Coordinate(
                                    Convert.ToSingle(e.TotalX),
                                    Convert.ToSingle(e.TotalY)).Add(_lastDisplayOffset ?? new Coordinate());
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

        CursorDebugOutput = string.Join(Environment.NewLine, _cursorDebugValues.Select(kv => kv.Key + "=" + kv.Value));
        CursorDebugChanged?.Invoke();
    }

    private async void TapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        Point touch = e.GetPosition(_graphicsView) ?? Point.Zero;
        SetCursorDebugValue("Tap", touch.X + "/" + touch.Y);
        _cursorPosition = touch;
        await API.Call(async () =>
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

                if (API.Com<bool>("Items", "IsInserting"))
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
                        if (CurrentWorksheet.IsSelected(selectedItem))
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
                                if (pinX != x || pinY != y)
                                {
                                    continue;
                                }

                                if (CurrentWorksheet.SelectedPin == pin)
                                {
                                    CurrentWorksheet.SelectedPin = null;
                                    await Paint();
                                    return;
                                }

                                if (CurrentWorksheet.SelectedPin != null)
                                {
                                    await AddTrace(pin, CurrentWorksheet.SelectedPin);
                                }

                                selectedPin = pin;
                                CurrentWorksheet.SelectedPin = selectedPin;

                                break;
                            }

                            if (selectedPin == null)
                            {
                                CurrentWorksheet.ToggleSelectItem(selectedItem);
                            }
                        }
                        else
                        {
                            CurrentWorksheet.ToggleSelectItem(selectedItem);
                        }

                        await Paint();
                    }
                }
            }
        });
    }
}