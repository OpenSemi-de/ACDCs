using System.Collections.ObjectModel;
using OSECircuitRender;
using OSECircuitRender.Definitions;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using OSECircuitRender.Scene;

namespace OSEInventory;

public partial class SheetPage
{
    private readonly List<WorksheetItem> _allItems = new();
    private readonly ObservableCollection<WorksheetItem> _allItemsObservable = new();
    private readonly List<WorksheetItem> _selectedItems = new();
    private readonly ObservableCollection<WorksheetItem> _selectedItemsObservable = new();
    private Rect? _allItemsBounds;
    private Point _cursorPosition;
    private PointF _dragStartPosition;
    private Dictionary<WorksheetItem, Coordinate> _selectedItemsBasePositions;
    private Rect? _selectedItemsBounds;
    private Rect _zoomSliderFrameBouds;

    public SheetPage()
    {
        InitializeComponent();
        SetupPage().Wait();
        SetupSheet();
    }

    public bool IsAllItemsVisible { get; set; } = true;
    public bool IsDraggingItem { get; set; }
    public bool IsSelectedItemsVisible { get; set; } = true;
    public Coordinate? LastDisplayOffset { get; set; }

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

    private async void BnShowHideAllItems_OnClicked(object? sender, EventArgs e)
    {
        await App.Call(ToggleAllItemsVisibility);
    }

    private async void BnShowHideSelectedItems_OnClicked(object? sender, EventArgs e)
    {
        await App.Call(ToggleSelectedItemsVisibility);
    }

    private WorksheetItem? GetWorksheetItemaAt(PointF position)
    {
        WorksheetItem? selectedItem = null;
        App.Call(() =>
        {
            float x = GetRelPos(position.X);
            float y = GetRelPos(position.Y);

            var hitItems = App.CurrentSheet.Items.Where(
                item =>
                    x >= item.X && x <= item.X + item.Width &&
                    y >= item.Y && y <= item.Y + item.Height

            );

            selectedItem = (WorksheetItem?)hitItems.First();
            return Task.CompletedTask;
        }).Wait();

        return selectedItem;
    }

    private void OnSheetItemAdded(IWorksheetItem obj)
    {
        _allItemsObservable.Clear();
        _allItems.Clear();
        _allItems.AddRange(App.CurrentSheet?.Items.Cast<WorksheetItem>() ?? Array.Empty<WorksheetItem>());
        _allItems.OrderBy(i => i.RefName).ToList().ForEach(i => _allItemsObservable.Add(i));
        lvAllItems.ItemsSource = _allItemsObservable;

        Paint().Wait();
    }

    private async Task Paint()
    {
        await App.Call(() =>
        {
            if (ItemButtonView.IsInserting)
                return Task.CompletedTask;

            sheetGraphicsView.Drawable = null;
            App.CurrentSheet?.CalculateScene();
            DrawableScene? scene = (DrawableScene?)
                App.CurrentSheet?.SceneManager?.GetSceneForBackend();
            sheetGraphicsView.Drawable = scene;

            sheetGraphicsView.Invalidate();
            return Task.CompletedTask;
        });
    }

    private void PanGestureRecognizer_OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        App.Call(async () =>
        {
            if (e.StatusType == GestureStatus.Started || e.StatusType == GestureStatus.Completed)
            {
                LastDisplayOffset = App.CurrentSheet?.DisplayOffset ??
                                    new Coordinate(
                                        Convert.ToSingle(e.TotalX),
                                        Convert.ToSingle(e.TotalY));
                if (e.StatusType == GestureStatus.Started)
                {
                    _selectedItemsBasePositions = new(_selectedItems.Select(selectedItem =>
                        new KeyValuePair<WorksheetItem, Coordinate>(selectedItem,
                           new Coordinate(selectedItem.DrawableComponent.Position))));

                    _dragStartPosition = new PointF(Convert.ToSingle(_cursorPosition.X - LastDisplayOffset?.X),
                        Convert.ToSingle(_cursorPosition.Y - LastDisplayOffset?.Y));
                    var testForItem = GetWorksheetItemaAt(_dragStartPosition);
                    if (testForItem != null)
                    {
                        IsDraggingItem = true;
                    }
                }

                if (e.StatusType == GestureStatus.Completed)
                    IsDraggingItem = false;
            }

            if (e.StatusType == GestureStatus.Running)
            {
                if (App.CurrentSheet != null)
                {
                    if (IsDraggingItem)
                    {
                        var cursorPosition = new PointF(Convert.ToSingle(_cursorPosition.X - LastDisplayOffset?.X),
                             Convert.ToSingle(_cursorPosition.Y - LastDisplayOffset?.Y));

                        PointF differenceBetweenCursorPoints = new(_dragStartPosition.X - cursorPosition.X,
                                _dragStartPosition.Y - cursorPosition.Y);
                        _selectedItems.ForEach(item =>
                            {
                                if (item != null)
                                {
                                    Point newPosition = new()
                                    {
                                        X = _selectedItemsBasePositions[item].X,
                                        Y = _selectedItemsBasePositions[item].Y
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
                        App.CurrentSheet.DisplayOffset =
                                new Coordinate(
                                    Convert.ToSingle(e.TotalX),
                                    Convert.ToSingle(e.TotalY)).Add(LastDisplayOffset ?? new Coordinate());
                    }
                }

                await Paint();
            }
        }).Wait();
    }

    private void PanGestureRecognizerZoom_OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        App.Call(async () =>
        {
            if (e.StatusType == GestureStatus.Started || e.StatusType == GestureStatus.Completed)
            {
                _zoomSliderFrameBouds = AbsoluteLayout.GetLayoutBounds(ZoomSliderFrame);
            }
            else
            {
                Rect offset = _zoomSliderFrameBouds.Offset(0, e.TotalY);
                SetZoomToPoint(Convert.ToSingle(offset.Y));
                await Paint();
            }
        }).Wait();
    }

    private void PointerGestureRecognizer_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        _cursorPosition = e.GetPosition(sheetGraphicsView) ?? new Point();
    }

    private async Task SetupPage()
    {
        await App.Call(async () =>
        {
            await App.Call(ItemButtonView.SetupView);
        });
    }

    private async void SetupSheet()
    {
        await App.Call(async () =>
        {
            App.CurrentWorkbook ??= new Workbook();

            App.CurrentSheet = App.CurrentWorkbook.AddNewSheet();
            App.CurrentSheet.Items.OnItemAdded = OnSheetItemAdded;

            App.CurrentSheet.CalculateScene();
            DrawableScene? scene = (DrawableScene?)
                App.CurrentSheet.SceneManager?.GetSceneForBackend();

            if (scene != null)
            {
                sheetGraphicsView.Drawable = scene;
            }

            await ToggleSelectedItemsVisibility();
            await ToggleAllItemsVisibility();
        });
    }

    private void SetZoomToPoint(float y)
    {
        if (y < 0)
        {
            y = 0;
        }

        if (y > 339)
        {
            y = 339;
        }

        AbsoluteLayout.SetLayoutBounds(ZoomSliderFrame,
            new Rect(0, y, 60, 60)
        );

        double zoom = 20 * ((y + 10) / 340);

        Workbook.Zoom = Convert.ToSingle(zoom);

        // todo: rescale the displayoffset
    }

    private async void TapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        await App.Call(async () =>
        {
            Point touch = e.GetPosition(sheetGraphicsView) ?? Point.Zero;
            if (touch != Point.Zero)
            {
                float offsetX = 0;
                float offsetY = 0;
                if (LastDisplayOffset != null)
                {
                    offsetX = LastDisplayOffset.X;
                    offsetY = LastDisplayOffset.Y;
                }

                if (ItemButtonView.IsInserting)
                {
                    float x = GetRelPos(touch.X - offsetX);
                    float y = GetRelPos(touch.Y - offsetY);
                    await ItemButtonView.InsertToPosition(x, y);
                    await Paint();
                }
                else
                {
                    touch.X -= offsetX;
                    touch.Y -= offsetY;

                    WorksheetItem? selectedItem = GetWorksheetItemaAt(touch);

                    if (selectedItem != null)
                    {
                        if (App.CurrentSheet != null && App.CurrentSheet.ToggleSelectItem(selectedItem))
                        {
                            if (!_selectedItems.Contains(selectedItem))
                                _selectedItems.Add(selectedItem);
                        }
                        else
                        {
                            if (_selectedItems.Contains(selectedItem))
                                _selectedItems.Remove(selectedItem);
                        }

                        _selectedItemsObservable.Clear();
                        _selectedItems.OrderBy(i => i.RefName).ToList()
                            .ForEach(i => _selectedItemsObservable.Add(i));
                        lvSelectedItems.ItemsSource = _selectedItemsObservable;

                        await Paint();
                    }
                }
            }
        });
    }

    private void TapGestureRecognizerSheetPage_OnTapped(object? sender, TappedEventArgs e)
    {
        MenuButtonView.HideAllMenus();
    }

    private void TapGestureRecognizerZoom_OnTapped(object? sender, TappedEventArgs e)
    {
        App.Call(async () =>
        {
            Point clickPoint = e.GetPosition(AbsoluteLayoutZoom) ?? Point.Zero;
            float y = Convert.ToSingle(clickPoint.Y);
            y -= 30;

            SetZoomToPoint(y);
            await Paint();
        }).Wait();
    }

    private async Task ToggleAllItemsVisibility()
    {
        await App.Call(async () =>
        {
            _allItemsBounds ??= AbsoluteLayout.GetLayoutBounds(fAllItems);

            switch (IsAllItemsVisible)
            {
                case true:

                    var bounds = new Rect((Point)_allItemsBounds?.Location, (Size)_allItemsBounds?.Size)
                    {
                        Width = 30,
                        Height = 30
                    };

                    await fAllItems.LayoutTo(bounds, 500U, Easing.Linear);
                    AbsoluteLayout.SetLayoutBounds(fAllItems, bounds);

                    bnShowHideAllItems.Text = "<";
                    IsAllItemsVisible = false;
                    break;

                default:

                    await fAllItems.LayoutTo(_allItemsBounds.Value, 500U, Easing.Linear);
                    AbsoluteLayout.SetLayoutBounds(fAllItems, _allItemsBounds.Value);
                    bnShowHideAllItems.Text = ">";
                    IsAllItemsVisible = true;
                    break;
            }
        });
    }

    private async Task ToggleSelectedItemsVisibility()
    {
        await App.Call(async () =>
        {
            _selectedItemsBounds ??= AbsoluteLayout.GetLayoutBounds(fSelectedItems);

            switch (IsSelectedItemsVisible)
            {
                case true:

                    var bounds = new Rect((Point)_selectedItemsBounds?.Location, (Size)_selectedItemsBounds?.Size)
                    {
                        Width = 30,
                        Height = 30
                    };

                    await fSelectedItems.LayoutTo(bounds, 500U, Easing.Linear);
                    AbsoluteLayout.SetLayoutBounds(fSelectedItems, bounds);

                    bnShowHideSelectedItems.Text = "<";
                    IsSelectedItemsVisible = false;
                    break;

                default:

                    await fSelectedItems.LayoutTo(_selectedItemsBounds.Value, 500U, Easing.Linear);
                    AbsoluteLayout.SetLayoutBounds(fSelectedItems, _selectedItemsBounds.Value);
                    bnShowHideSelectedItems.Text = ">";
                    IsSelectedItemsVisible = true;
                    break;
            }
        });
    }
}
