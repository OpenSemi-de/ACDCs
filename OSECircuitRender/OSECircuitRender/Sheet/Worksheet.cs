#nullable enable

using AnyClone;
using Newtonsoft.Json;
using OSECircuitRender.Definitions;
using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using OSECircuitRender.Scene;
using System;
using System.Linq;

namespace OSECircuitRender.Sheet;

public sealed class Worksheet
{
    public Worksheet()
    {
        Router = new TwoDPathRouter(SheetSize, GridSize);
        Items.OnAdded(OnItemAdded);
        SelectedItems.OnAdded(OnSelectionAdded);
    }

    public Color? BackgroundColor { get; set; }

    public Coordinate? DisplayOffset { get; set; }

    public float GridSize { get; set; } = 2.54f;

    public WorksheetItemList Items { get; set; } = new();

    public WorksheetItemList Nets { get; set; } = new();

    public Action<WorksheetItemList>? OnSelectionChange { get; set; }

    public IPathRouter Router { get; set; }

    [JsonIgnore] public ISceneManager? SceneManager { get; set; }

    public WorksheetItemList SelectedItems { get; set; } = new();

    public int SheetNum { get; set; }

    public Coordinate SheetSize { get; set; } = new(100, 100, 0);

    public bool ShowGrid { get; set; } = true;

    public WorksheetItemList Traces { get; set; } = new();

    public bool CalculateScene()
    {
        Log.L("Calculating scene");

        Traces = Router.GetTraces();

        if (SceneManager == null)
        {
            Log.L("Creating Manager");
            SceneManager = new DefaultSceneManager();
            SceneManager.ShowGrid = ShowGrid;
            SceneManager.BackgroundColor = BackgroundColor;
        }

        if (SceneManager.SetScene(GetDrawableComponents(), GetSelectedComponents()))
        {
            SceneManager.DisplayOffset = DisplayOffset;

            SceneManager.SetSizeAndScale(SheetSize, GridSize);
            Log.L("Getting backend scene");
            var backendScene = SceneManager.GetSceneForBackend();
            if (backendScene != null)
                if (SceneManager.SendToBackend(backendScene))
                {
                    Log.L("Backend received scene");
                    return true;
                }
        }

        return false;
    }

    public void DeleteItem(WorksheetItem item)
    {
        if (SelectedItems.Contains(item))
            SelectedItems.Remove(item);

        if (Items.Contains(item))
            Items.Remove(item);
    }

    public void DeselectItem(WorksheetItem item)
    {
        if (SelectedItems.Contains(item))
        {
            SelectedItems.Remove(item);
        }
    }

    public WorksheetItem DuplicateItem(WorksheetItem item)
    {
        WorksheetItem? newItem = item.Clone<WorksheetItem>();
        return newItem;
    }

    public DrawableComponentList GetDrawableComponents()
    {
        return new DrawableComponentList(
            Items
                .Select(item =>
                    item.DrawableComponent
                )
                .Union(
                    Traces
                        .Select(item =>
                            item.DrawableComponent
                        )
                ));
    }

    public WorksheetItem? GetItemAt(float x, float y)
    {
        int iX = Convert.ToInt32(Math.Round(x));
        int iY = Convert.ToInt32(Math.Round(y));

        var selectedItem = Items.FirstOrDefault(
            item => item.X == iX && item.Y == iY);

        return (WorksheetItem?)selectedItem;
    }

    public void SelectItem(WorksheetItem item)
    {
        if (!SelectedItems.Contains(item))
        {
            SelectedItems.AddItem(item);
        }
    }

    public bool ToggleSelectItem(WorksheetItem selectedItem)
    {
        if (SelectedItems.Contains(selectedItem))
        {
            SelectedItems.Remove(selectedItem);
            return false;
        }
        else
        {
            SelectedItems.AddItem(selectedItem);
            return true;
        }
    }

    private DrawableComponentList GetSelectedComponents()
    {
        return new DrawableComponentList(
            SelectedItems.Select(item => item.DrawableComponent)
        );
    }

    private void OnItemAdded(IWorksheetItem item)
    {
        Router.SetItems(Items, Nets);
    }

    private void OnSelectionAdded(IWorksheetItem obj)
    {
        OnSelectionChange?.Invoke(SelectedItems);
    }
}