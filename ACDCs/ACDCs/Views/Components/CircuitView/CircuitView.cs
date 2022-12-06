using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using OSECircuitRender;
using OSECircuitRender.Scene;
using OSECircuitRender.Sheet;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Graphics;
using Newtonsoft.Json;
using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace ACDCs.Views.Components.CircuitView;

public class CircuitView : ContentView
{
    private readonly GraphicsView _graphicsView;
    private readonly PanGestureRecognizer _panRecognizer;
    private readonly TapGestureRecognizer _tapRecognizer;
    private readonly PointerGestureRecognizer _pointerRecognizer;
    private readonly Workbook _currentWorkbook;
    private Dictionary<WorksheetItem, Coordinate> _selectedItemsBasePositions;
    private Worksheet _currentSheet;
    private Coordinate? _lastDisplayOffset;
    private PointF _dragStartPosition;
    private bool _isDraggingItem;
    private Point _cursorPosition;
    private Action<WorksheetItemList, WorksheetItemList> ListSetItems { get => App.Com<Action<WorksheetItemList, WorksheetItemList>>("ItemList", "SetItems"); }
    public Worksheet CurrentWorksheet
    {
        get => _currentSheet;
    }

    public CircuitView()
    {
        _currentWorkbook = new Workbook();
        _currentSheet = _currentWorkbook.AddNewSheet();
        _graphicsView = new GraphicsView()
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
        };

        _tapRecognizer = new TapGestureRecognizer();
        _tapRecognizer.Tapped += TapGestureRecognizer_OnTapped;

        _panRecognizer = new PanGestureRecognizer();
        _panRecognizer.PanUpdated += PanGestureRecognizer_OnPanUpdated;

        _pointerRecognizer = new PointerGestureRecognizer();
        _pointerRecognizer.PointerMoved += PointerGestureRecognizer_OnPointerMoved;

        _graphicsView.GestureRecognizers.Add(_tapRecognizer);
        _graphicsView.GestureRecognizers.Add(_panRecognizer);
        _graphicsView.GestureRecognizers.Add(_pointerRecognizer);

        Content = _graphicsView;
        App.Com<CircuitView>(nameof(CircuitView), "Instance", this);
        App.Com<Worksheet>(nameof(CircuitView), "CurrentWorksheet", _currentSheet);
        Paint();
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

            ListSetItems(_currentSheet.Items, _currentSheet.SelectedItems);

            return Task.CompletedTask;
        });
    }

    private async void TapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        await App.Call(async () =>
        {
            Point touch = e.GetPosition(_graphicsView) ?? Point.Zero;
            if (touch != Point.Zero)
            {
                float offsetX = 0;
                float offsetY = 0;
                if (_lastDisplayOffset != null)
                {
                    offsetX = _lastDisplayOffset.X;
                    offsetY = _lastDisplayOffset.Y;
                }

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
                        if (_currentSheet.IsSelected(selectedItem))
                        {

                            float x = GetRelPos(touch.X);
                            float y = GetRelPos(touch.Y);

                            PinDrawable? selectedPin = null;
                            foreach (var pin in selectedItem.Pins)
                            {
                                var pinX = Math.Floor(pin.Position.X * selectedItem.Width);
                                var pinY = Math.Floor(pin.Position.Y * selectedItem.Height);
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


    private static float GetRelPos(double pos)
    {
        var relPos = 0f;
        App.Call(() =>
        {
            relPos = Convert.ToSingle(Math.Round(pos / (Workbook.BaseGridSize * Workbook.Zoom)));
            return Task.CompletedTask;
        }).Wait();
        return relPos;
    }

    private WorksheetItem? GetWorksheetItemaAt(PointF position)
    {
        WorksheetItem? selectedItem = null;
        App.Call(() =>
        {
            float x = GetRelPos(position.X);
            float y = GetRelPos(position.Y);

            var hitItems = _currentSheet.Items.Where(
                item =>
                    x >= item.X && x <= item.X + item.Width &&
                    y >= item.Y && y <= item.Y + item.Height

            );
            var worksheetItems = hitItems as IWorksheetItem[] ?? hitItems.ToArray();
            if (worksheetItems.Any())
                selectedItem = (WorksheetItem?)worksheetItems.First();
            return Task.CompletedTask;
        }).Wait();

        return selectedItem;
    }


    public async Task Paint()
    {
        await App.Call(() =>
        {
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

    private void PanGestureRecognizer_OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        App.Call(async () =>
        {
            if (e.StatusType == GestureStatus.Started || e.StatusType == GestureStatus.Completed)
            {
                _lastDisplayOffset = _currentSheet?.DisplayOffset ??
                                     new Coordinate(
                                         Convert.ToSingle(e.TotalX),
                                         Convert.ToSingle(e.TotalY));
                if (e.StatusType == GestureStatus.Started)
                {
                    _selectedItemsBasePositions = new(_currentSheet.SelectedItems.Select(selectedItem =>
                        new KeyValuePair<WorksheetItem, Coordinate>((WorksheetItem)selectedItem,
                           new Coordinate(selectedItem.DrawableComponent.Position))));

                    _dragStartPosition = new PointF(Convert.ToSingle(_cursorPosition.X - _lastDisplayOffset?.X),
                        Convert.ToSingle(_cursorPosition.Y - _lastDisplayOffset?.Y));
                    var testForItem = GetWorksheetItemaAt(_dragStartPosition);
                    if (testForItem != null)
                    {
                        _isDraggingItem = true;
                    }
                }

                if (e.StatusType == GestureStatus.Completed)
                    _isDraggingItem = false;
            }

            if (e.StatusType == GestureStatus.Running)
            {
                if (_currentSheet != null)
                {
                    if (_isDraggingItem)
                    {
                        var cursorPosition = new PointF(Convert.ToSingle(_cursorPosition.X - _lastDisplayOffset?.X),
                             Convert.ToSingle(_cursorPosition.Y - _lastDisplayOffset?.Y));

                        PointF differenceBetweenCursorPoints = new(_dragStartPosition.X - cursorPosition.X,
                                _dragStartPosition.Y - cursorPosition.Y);
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

                                item.X = Convert.ToInt32(newPosition.X);
                                item.Y = Convert.ToInt32(newPosition.Y);
                            }
                        }
                        );
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
            }
        }).Wait();
    }

    private void PointerGestureRecognizer_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        _cursorPosition = e.GetPosition(_graphicsView) ?? new Point();
    }


    public async void SaveAs(string fileName)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All
        };
        string jsonData = JsonConvert.SerializeObject(_currentSheet, settings: settings);
        _currentSheet.Filename = Path.GetFileName(fileName);
        await File.WriteAllTextAsync(fileName, jsonData);
        OnSavedSheet();
    }

    public async void Open(string fileName)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings()
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

            App.Com<Worksheet>(nameof(CircuitView), "CurrentWorksheet", _currentSheet);
            _currentSheet.Filename = Path.GetFileName(fileName);
            Paint();
        }

        OnLoadedSheet();
    }

    private void JsonError(object? sender, ErrorEventArgs e)
    {
        Console.WriteLine(e.ErrorContext.Error.ToString());
    }

    public void Clear()
    {

        _currentWorkbook.Sheets.Clear();
        _currentSheet = _currentWorkbook.AddNewSheet();
    }

    public event EventHandler<EventArgs> LoadedSheet;

    protected virtual void OnLoadedSheet()
    {
        LoadedSheet?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<EventArgs> SavedSheet;

    protected virtual void OnSavedSheet()
    {
        SavedSheet?.Invoke(this, EventArgs.Empty);
    }
}