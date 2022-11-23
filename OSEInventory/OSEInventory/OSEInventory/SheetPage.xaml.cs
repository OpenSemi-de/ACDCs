using System.Dynamic;
using System.Reflection;
using OSECircuitRender;
using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using OSECircuitRender.Scene;
using OSEInventory.Components;
using Color = Microsoft.Maui.Graphics.Color;

namespace OSEInventory;

public partial class SheetPage
{
    private List<WorksheetItem> selectedItems = new();

    public SheetPage()
    {
        InitializeComponent();
        SetupPage();
        SetupSheet();
    }

    private void SetupPage()
    {
        foreach (var type in typeof(IWorksheetItem).Assembly.GetTypes())
        {
            TypeFilter typeFilter = new((filterType, criteria) => filterType == typeof(IWorksheetItem));
            if (type.FindInterfaces(typeFilter, null).Length > 0)
            {
                var IsInsertableProp = type.GetProperty("IsInsertable");
                if (IsInsertableProp != null)
                {
                    bool IsInsertable = (bool)(IsInsertableProp.GetValue(null, BindingFlags.Static, null, null, null) ?? false);
                    if (IsInsertable)
                    {
                        ItemButton button = new(type);
                        button.Clicked += OnItemButtonClicked;
                        slComponentButtons.Add(
                            button
                        );
                    }
                }
            }
        }
    }



    private void OnItemButtonClicked(object? sender, EventArgs e)
    {
        if (sender is ItemButton button)
        {
            if (button.ItemType != null)
            {
                InsertItem(button.ItemType, button);
            }
        }
    }

    public ItemButton? SelectedButton { get; set; }

    public Color SelectedButtonColor { get; set; }

    public Func<float, float, WorksheetItem> DoInsert { get; set; }

    public bool IsInserting { get; set; }

    private void SetupSheet()
    {
        if (App.CurrentWorkbook == null)
        {
            App.CurrentWorkbook = new Workbook();
        }

        App.CurrentSheet = App.CurrentWorkbook.AddNewSheet();
        App.CurrentSheet.Items.OnItemAdded = OnSheetItemAdded;


        App.CurrentSheet.CalculateScene();
        DrawableScene scene = (DrawableScene)
            App.CurrentSheet.SceneManager.GetSceneForBackend();


        sheetGraphicsView.Drawable = scene;
    }

    private void OnSheetItemAdded(IWorksheetItem obj)
    {
        Paint();
    }

    private void Paint()
    {
        if (IsInserting)
            return;
        App.CurrentSheet.CalculateScene();
        DrawableScene scene = (DrawableScene)
            App.CurrentSheet.SceneManager.GetSceneForBackend();
        sheetGraphicsView.Drawable = scene;

        sheetGraphicsView.Invalidate();
    }


    private void InsertItem(Type itemType, ItemButton selectedButton)
    {
        bool justDeselectAndReturn = SelectedButton == selectedButton;
        IsInserting = false;
        DoInsert = (x, y) => null;
        DeselectSelectedButton();
        if (justDeselectAndReturn) return;

        SelectButton(selectedButton);

        if (Activator.CreateInstance(itemType) is WorksheetItem item)
        {
            Insert(item);
        }

        if (!IsInserting)
            DeselectSelectedButton();
    }

    private void Insert(WorksheetItem item)
    {
        DoInsert = ((float x, float y) =>
        {
            item.DrawableComponent.Position.X = x;
            item.DrawableComponent.Position.Y = y;
            App.CurrentSheet.Items.AddItem(item);
            return item;
        });
        IsInserting = true;
    }

    private void SelectButton(ItemButton selectedButton)
    {
        SelectedButton = selectedButton;
        SelectedButtonColor = SelectedButton?.BackgroundColor;
        if (SelectedButton != null)
        {
            SelectedButton.BackgroundColor = Application.AccentColor;
        }
    }


    private void SheetGraphicsView_OnStartInteraction(object sender, TouchEventArgs e)
    {

    }

    private float GetRelPos(double pos)
    {
        return Convert.ToSingle(Math.Round(pos / (Workbook.BaseGridSize * Workbook.Zoom)));
    }

    private void DeselectSelectedButton()
    {
        if (SelectedButton != null)
        {
            SelectedButton.BackgroundColor = SelectedButtonColor;
        }

        SelectedButton = null;
    }

    private void SheetGraphicsView_OnEndInteraction(object sender, TouchEventArgs e)
    {
        if (e.Touches.Length > 0)
        {
            if (IsInserting)
            {
                var touch = e.Touches[0];
                var newItem = DoInsert?.Invoke(
                    GetRelPos(touch.X),
                    GetRelPos(touch.Y));

                DeselectSelectedButton();
                IsInserting = false;
                Paint();
            }
            else
            {
                var touch = e.Touches[0];
                WorksheetItem? selectedItem = App.CurrentSheet.GetItemAt(
                    GetRelPos(touch.X),
                    GetRelPos(touch.Y)
                );

                selectedItem = WorksheetItem(selectedItem, touch);


                if (selectedItem != null)
                {
                    if (App.CurrentSheet.ToggleSelectItem(selectedItem))
                    {
                        if (!selectedItems.Contains(selectedItem))
                            selectedItems.Add(selectedItem);
                    }
                    else
                    {
                        if (selectedItems.Contains(selectedItem))
                            selectedItems.Remove(selectedItem);
                    }
                    Paint();
                }
            }

        }
    }

    private WorksheetItem WorksheetItem(WorksheetItem selectedItem, PointF touch)
    {
        List<int> offsets = new() { 0, -1, 1 };

        foreach (int row in offsets)
            foreach (int column in offsets)
            {
                if (selectedItem == null)
                {
                    selectedItem = App.CurrentSheet.GetItemAt(
                        GetRelPos(touch.X) + column,
                        GetRelPos(touch.Y) + row);
                }
                else break;
            }

        return selectedItem;
    }


    private void PanGestureRecognizer_OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Started)
        {
            LastDisplayOffset = App.CurrentSheet.DisplayOffset;
        }

        if (e.StatusType == GestureStatus.Running)
        {
            App.CurrentSheet.DisplayOffset =
                new Coordinate(
                    Convert.ToSingle(e.TotalX),
                    Convert.ToSingle(e.TotalY)).Add(LastDisplayOffset ?? new Coordinate());
            Paint();
        }
    }

    public Coordinate LastDisplayOffset { get; set; }
}
