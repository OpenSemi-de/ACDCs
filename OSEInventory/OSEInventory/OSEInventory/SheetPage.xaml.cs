using System.Collections.ObjectModel;
using System.Reflection;
using OSECircuitRender;
using OSECircuitRender.Definitions;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using OSECircuitRender.Scene;
using OSEInventory.Components;
using Application = Microsoft.Maui.Controls.Application;
using Color = Microsoft.Maui.Graphics.Color;

namespace OSEInventory;

public partial class SheetPage
{
    private readonly List<WorksheetItem> _selectedItems = new();
    private readonly ObservableCollection<WorksheetItem> _selectedItemsObservable = new();
    private readonly ObservableCollection<WorksheetItem> _allItemsObservable = new();
    private Rect? _allItemsBounds;
    private Action<string>? _log;
    private Rect? _selectedItemsBounds;
    private readonly List<WorksheetItem> _allItems = new();

    public SheetPage()
    {
        InitializeComponent();
        SetupPage();
        SetupSheet();
        _log = Console.WriteLine;
    }

    public Func<float, float, WorksheetItem?>? DoInsert { get; set; }

    public bool IsAllItemsVisible { get; set; }
    public bool IsInserting { get; set; }

    public bool IsSelectedItemsVisible { get; set; } = true;
    public Coordinate? LastDisplayOffset { get; set; }

    public ItemButton? SelectedButton { get; set; }

    public Color? SelectedButtonBorderColor { get; set; }

    public Color? SelectedButtonColor { get; set; }

    private async void BnShowHideAllItems_OnClicked(object? sender, EventArgs e)
    {
        await Try(ToggleAllItemsVisibility);
    }

    private async void BnShowHideSelectedItems_OnClicked(object? sender, EventArgs e)
    {
        await Try(ToggleSelectedItemsVisibility);
    }

    private async Task DeselectSelectedButton()
    {
        await Try(() =>
        {
            if (SelectedButton != null)
            {
                SelectedButton.BackgroundColor = SelectedButtonColor;
            }

            SelectedButton = null;
            return Task.CompletedTask;
        });
    }

    private float GetRelPos(double pos)
    {
        float relPos = 0f;
        Try(() =>
        {
            relPos = Convert.ToSingle(Math.Round(pos / (Workbook.BaseGridSize * Workbook.Zoom)));
            return Task.CompletedTask;
        }).Wait();
        return relPos;
    }

    private WorksheetItem? GetWorksheetItemaAt(PointF position)
    {
        WorksheetItem? selectedItem = null;
        Try(() =>
        {
            selectedItem = App.CurrentSheet?.GetItemAt(
                GetRelPos(position.X),
                GetRelPos(position.Y)
            );

            List<int> offsets = new() { 0, -1, 1 };

            foreach (int row in offsets)
                foreach (int column in offsets)
                {
                    if (selectedItem == null)
                    {
                        selectedItem = App.CurrentSheet?.GetItemAt(
                            GetRelPos(position.X) + column,
                            GetRelPos(position.Y) + row);
                    }
                    else break;
                }
            return Task.CompletedTask;
        }).Wait();

        return selectedItem;
    }

    private async Task Insert(WorksheetItem? item)
    {
        await Try(() =>
        {
            if (item != null)
            {
                DoInsert = (x, y) =>
                {
                    item.DrawableComponent.Position.X = x;
                    item.DrawableComponent.Position.Y = y;
                    App.CurrentSheet?.Items.AddItem(item);
                    return item;
                };
            }

            IsInserting = true;
            return Task.CompletedTask;
        });
    }

    private async Task InsertItem(Type itemType, ItemButton selectedButton)
    {
        await Try(async () =>
        {
            bool justDeselectAndReturn = SelectedButton == selectedButton;
            IsInserting = false;
            DoInsert = (x, y) => null;
            await DeselectSelectedButton();
            if (justDeselectAndReturn) return;

            await SelectButton(selectedButton);

            if (Activator.CreateInstance(itemType) is WorksheetItem item)
            {
                await Insert(item);
            }

            if (!IsInserting)
                await DeselectSelectedButton();
        });
    }

    private void OnItemButtonClicked(object? sender, EventArgs e)
    {
        Try(async () =>
        {
            if (sender is ItemButton button)
            {
                if (button.ItemType != null)
                {
                    await InsertItem(button.ItemType, button);
                }
            }
        }).Wait();
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
        await Try(() =>
        {
            if (IsInserting)
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
        
        Try(async () =>
         {
             
             if (e.StatusType == GestureStatus.Started || e.StatusType == GestureStatus.Completed)
             {
                 LastDisplayOffset = App.CurrentSheet?.DisplayOffset;
             }

             if (e.StatusType == GestureStatus.Running)
             {
                 if (App.CurrentSheet != null)
                 {
                     App.CurrentSheet.DisplayOffset =
                         new Coordinate(
                             Convert.ToSingle(e.TotalX),
                             Convert.ToSingle(e.TotalY)).Add(LastDisplayOffset ?? new Coordinate());
                 }

                 await Paint();
             }
         }).Wait();
    }

    private async Task SelectButton(ItemButton selectedButton)
    {
        await Try(() =>
        {
            SelectedButton = selectedButton;
            SelectedButtonColor = SelectedButton?.BackgroundColor;
            SelectedButtonBorderColor = SelectedButton?.BorderColor;
            if (SelectedButton != null)
            {
                SelectedButton.BorderColor = Colors.SlateGray;
                SelectedButton.BackgroundColor = Application.AccentColor;
            }

            return Task.CompletedTask;
        });
    }

    private async Task SetupPage()
    {
        await Try(() =>
        {
            foreach (Type type in typeof(IWorksheetItem).Assembly.GetTypes())
            {
                bool TypeFilter(Type filterType, object? criteria) => filterType == typeof(IWorksheetItem);

                if (type.FindInterfaces(TypeFilter, null).Length <= 0)
                {
                    continue;
                }

                PropertyInfo? isInsertableProp = type.GetProperty("IsInsertable");
                if (isInsertableProp == null)
                {
                    continue;
                }

                bool isInsertable =
                    (bool)(isInsertableProp.GetValue(null, BindingFlags.Static, null, null, null) ?? false);

                if (!isInsertable)
                {
                    continue;
                }

                ItemButton button = new(type) { WidthRequest = 60, HeightRequest = 60 };
                button.Clicked += OnItemButtonClicked;
                slComponentButtons.Add(
                    button
                );
            }

            return Task.CompletedTask;
        });
    }

    private async void SetupSheet()
    {
        await Try(async () =>
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
        });
    }

    private async void TapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        await Try(async () =>
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

                if (IsInserting)
                {
                    WorksheetItem? newItem = DoInsert?.Invoke(
                        GetRelPos(touch.X - offsetX),
                        GetRelPos(touch.Y - offsetY));
                    if (newItem != null)
                    {
                        newItem.X -= newItem.Width / 2;
                        newItem.Y -= newItem.Height / 2;
                    }

                    await DeselectSelectedButton();
                    IsInserting = false;
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
                        _selectedItems.OrderBy(i => i.RefName).ToList().ForEach(i => _selectedItemsObservable.Add(i));
                        lvSelectedItems.ItemsSource = _selectedItemsObservable;

                        await Paint();
                    }
                }
            }
        });
    }

    private async Task ToggleAllItemsVisibility()
    {
        await Try(async () =>
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
        await Try(async () =>
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

    private async Task Try(Func<Task> action)
    {
        try
        {
            await action().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _log?.Invoke(ex.ToString());
        }
    }
}
