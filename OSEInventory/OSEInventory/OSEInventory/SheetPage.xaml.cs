// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using OSECircuitRender;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using OSECircuitRender.Scene;

namespace OSEInventory;

public partial class SheetPage
{
    public SheetPage()
    {
        InitializeComponent();
        SetupSheet();
    }

    public Button? SelectedButton { get; set; }

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

    private void BnResistor_OnClicked(object sender, EventArgs e)
    {
        InsertItem<ResistorItem>(bnResistor);
    }

    private void InsertItem<T>(Button selectedButton)
    {
        bool justDeselectAndReturn = SelectedButton == selectedButton;
        IsInserting = false;
        DoInsert = (x, y) => null;
        DeselectSelectedButton();
        if (justDeselectAndReturn) return;

        SelectButton<T>(selectedButton);

        WorksheetItem? item = null;

        if (typeof(T) == typeof(ResistorItem))
        {
            item = new ResistorItem("10k");

        }
        else if (typeof(T) == typeof(CapacitorItem))
        {
            item = new CapacitorItem("10u", CapacitorDrawableType.Polarized);

        }
        else if (typeof(T) == typeof(InductorItem))
        {
            item = new InductorItem("1m");
        }
        else if (typeof(T) == typeof(DiodeItem))
        {
            item = new DiodeItem("");
        }

        if (item != null)
        {
            Insert<T>(item);
        }

        if (!IsInserting)
            DeselectSelectedButton();
    }

    private void Insert<T>(WorksheetItem item)
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

    private void SelectButton<T>(Button selectedButton)
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
        if (e.Touches.Length > 0)
        {
            if (IsInserting)
            {
                var touch = e.Touches[0];
                var newItem = DoInsert(touch.X / 25, touch.Y / 25);
                DeselectSelectedButton();
                IsInserting = false;
                Paint();
            }
        }
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
    }

    private void BnCapacitor_OnClicked(object sender, EventArgs e)
    {
        InsertItem<CapacitorItem>(bnCapacitor);
    }

    private void BnInductor_OnClicked(object sender, EventArgs e)
    {
        InsertItem<InductorItem>(bnInductor);
    }

    private void BnDiode_OnClicked(object sender, EventArgs e)
    {
        InsertItem<DiodeItem>(bnDiode);
    }
}
