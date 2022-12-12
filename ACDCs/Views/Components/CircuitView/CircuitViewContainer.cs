﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ACDCs.CircuitRenderer;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Scene;
using ACDCs.CircuitRenderer.Sheet;
using ACDCs.Views.Components.Feedback;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using Color = Microsoft.Maui.Graphics.Color;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace ACDCs.Views.Components.CircuitView;

public delegate void CursorPositionChangeEvent(object sender, CursorPositionChangeEventArgs args);

public class CircuitViewContainer : ContentView
{
    public static readonly BindableProperty BackgroundHighColorProperty =
        BindableProperty.Create(nameof(BackgroundHighColor), typeof(Color), typeof(CircuitSheetPage), null);

    public static readonly BindableProperty ForegroundColorProperty =
            BindableProperty.Create(nameof(ForegroundColor), typeof(Color), typeof(CircuitSheetPage), null);

    public CircuitViewContainer()
    {
        _currentWorkbook = new Workbook();
        _currentWorkbook.SetBaseFont("Maple Mono");
        _currentSheet = _currentWorkbook.AddNewSheet();
        BackgroundColor = Colors.Transparent;
        _graphicsView = new GraphicsView
        {
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
        };

        Feedback = new FeedbackFrame
        {
            IsVisible = false,
            BackgroundColor = Colors.Transparent,
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
        App.Com<CircuitViewContainer>(nameof(CircuitView), "Instance", this);
        App.Com<Worksheet>(nameof(CircuitView), "CurrentWorksheet", _currentSheet);
        Loaded += OnLoaded;
    }

    public event CursorPositionChangeEvent? CursorPositionChanged;

    public event EventHandler<EventArgs>? LoadedSheet;

    public event EventHandler<EventArgs>? SavedSheet;

    public event CursorPositionChangeEvent? TapPositionChanged;

    public Color? BackgroundHighColor
    {
        get => (Color)GetValue(BackgroundHighColorProperty);
        set => SetValue(BackgroundHighColorProperty, value);
    }

    public Worksheet CurrentWorksheet
    {
        get => _currentSheet;
    }

    public Color? ForegroundColor
    {
        get => (Color)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }

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
                newItem.X -= newItem.Width / 2;
                newItem.Y -= newItem.Height / 2;

                CurrentWorksheet.Items.AddItem(newItem);
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

            App.Com<Worksheet>(nameof(CircuitView), "CurrentWorksheet", _currentSheet);
            _currentSheet.Filename = Path.GetFileName(fileName);
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

            foreach (FeedbackRect feedbackRect in CurrentWorksheet.GetFeedbackRects() ?? new List<FeedbackRect>())
            {
                if (feedbackRect.Rect != null && feedbackRect.IsSelected)
                {
                    RectF rect = feedbackRect.Rect.Value;

                    float width =2*( rect.Width - rect.X);
                    float height =2*( rect.Height - rect.Y);
                    float rectX = rect.X - width / 2;
                    float rectY = rect.Y - height / 2;

                    AbsoluteLayout.SetLayoutBounds(Feedback, new Rect(rectX, rectY, width, height));
                    AbsoluteLayout.SetLayoutFlags(Feedback, AbsoluteLayoutFlags.None);
                }
            }

            return Task.CompletedTask;
        });
    }


    public AbsoluteLayout PopupTarget
    {
        get => (AbsoluteLayout)GetValue(PopupTargetProperty);

        set
        {
            SetValue(PopupTargetProperty, value);
            PutFeedback();
        }
    }

    private void PutFeedback()
    {

        PopupTarget.Add(Feedback);
    }

    private static readonly BindableProperty PopupTargetProperty =
        BindableProperty.Create(nameof(PopupTarget), typeof(AbsoluteLayout), typeof(CircuitSheetPage));


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
        _currentSheet.Filename = Path.GetFileName(fileName);
        await File.WriteAllTextAsync(fileName, jsonData);
        OnSavedSheet();
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

    private readonly Workbook _currentWorkbook;
    private readonly GraphicsView _graphicsView;

    public FeedbackFrame Feedback { get; }

    private readonly PanGestureRecognizer _panRecognizer;
    private readonly PointerGestureRecognizer _pointerRecognizer;
    private readonly TapGestureRecognizer _tapRecognizer;
    private Worksheet _currentSheet;
    private Point _cursorPosition;
    private PointF _dragStartPosition;
    private bool _isDraggingItem;
    private Coordinate? _lastDisplayOffset;
    private Dictionary<WorksheetItem, Coordinate> _selectedItemsBasePositions = new();

    private Action<WorksheetItemList, WorksheetItemList>? ListSetItems
    {
        get => App.Com<Action<WorksheetItemList, WorksheetItemList>>("ItemList", "SetItems");
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

    private async Task AddTrace(PinDrawable pinFrom, PinDrawable pinTo)
    {
        await App.Call(async () =>
        {
            IWorksheetItem? netFromPin = _currentSheet.Nets.FirstOrDefault(net => net.Pins.Contains(pinFrom));
            IWorksheetItem? netToPin = _currentSheet.Nets.FirstOrDefault(net => net.Pins.Contains(pinTo));
            if (netToPin == null & netFromPin == null)
            {
                _currentSheet.Nets.AddNet(pinFrom, pinTo);
            }

            if (netToPin == null && netFromPin != null)
            {
                netFromPin.Pins.Add(pinTo);
            }

            if (netToPin != null && netFromPin == null)
            {
                netToPin.Pins.Add(pinTo);
            }

            CurrentWorksheet.SelectedPin = null;
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
        App.Call(async () =>
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                case GestureStatus.Completed:
                    {
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
                            var testForItem = GetWorksheetItemaAt(_dragStartPosition);
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
                        break;
                    }
            }
        }).Wait();
    }

    private void PointerGestureRecognizer_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        _cursorPosition = e.GetPosition(_graphicsView) ?? new Point();
        OnCursorPositionChanged(new CursorPositionChangeEventArgs(_cursorPosition));
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

public class CursorPositionChangeEventArgs
{
    public CursorPositionChangeEventArgs(Point cursorPosition)
    {
        CursorPosition = cursorPosition;
    }

    public Point CursorPosition { get; }
}
