// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using OSECircuitRender;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using OSECircuitRender.Scene;

namespace OSEInventory;

public partial class SheetPage : ContentPage
{
    public SheetPage()
    {
        InitializeComponent();
        SetupSheet();
    }

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

    public Button? SelectedButton { get; set; }

    private void InsertItem<T>(Button selectedButton)
    {
        bool justDeselectAndReturn = SelectedButton == selectedButton;
        IsInserting = false;
        DoInsert = (x,y) => null;
        DeselectSelectedButton();
        if (justDeselectAndReturn) return;

        SelectButton<T>(selectedButton);

        if (typeof(T) == typeof(ResistorItem))
        {
            DoInsert = ((float x, float y) =>
            {
                var item = new ResistorItem("10k", x, y);
                App.CurrentSheet.Items.AddItem(item);
                return item;
            });
            IsInserting = true;
        }


        if (typeof(T) == typeof(CapacitorItem))
        {
            DoInsert = ((float x, float y) =>
            {
                var item = new CapacitorItem("10u", CapacitorDrawableType.Polarized, x, y);
                App.CurrentSheet.Items.AddItem(item);
                return item;
            });
            IsInserting = true;
        }

        if (!IsInserting)
            DeselectSelectedButton();
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

    public Color SelectedButtonColor { get; set; }

    public Func<float, float, WorksheetItem> DoInsert { get; set; }


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

    public bool IsInserting { get; set; }

    private void SheetGraphicsView_OnEndInteraction(object sender, TouchEventArgs e)
    {

    }

    private void BnCapacitor_OnClicked(object sender, EventArgs e)
    {
        InsertItem<CapacitorItem>(bnCapacitor);
    }
}
