﻿#nullable enable

using System;
using System.Linq;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Scene;
using AnyClone;
using Newtonsoft.Json;

namespace ACDCs.CircuitRenderer.Sheet;

public sealed class Worksheet
{
    public Worksheet()
    {
        Router = new TwoDPathRouter(this, SheetSize, GridSize);
        Nets = new WorksheetItemList(this);
        Items = new WorksheetItemList(this);
        Traces = new WorksheetItemList(this);
        SelectedItems = new WorksheetItemList(this);
        Items.OnAdded(OnItemAdded);
        SelectedItems.OnAdded(OnSelectionAdded);
    }

    public Color? BackgroundColor { get; set; }
    public Color? BackgroundHighColor { get; set; }
    public Coordinate? DisplayOffset { get; set; }
    public string Filename { get; set; } = "";
    public Color? ForegroundColor { get; set; }
    public float GridSize { get; set; } = 2.54f;
    public WorksheetItemList Items { get; set; }
    public WorksheetItemList Nets { get; set; }

    [JsonIgnore]
    public Action<WorksheetItemList>? OnSelectionChange { get; set; }

    [JsonIgnore]
    public IPathRouter Router { get; set; }

    [JsonIgnore]
    public ISceneManager? SceneManager { get; set; }

    public WorksheetItemList SelectedItems { get; set; }

    public PinDrawable? SelectedPin { get; set; }
    public int SheetNum { get; set; }

    public Coordinate SheetSize { get; set; } = new(100, 100, 0);

    public bool ShowGrid { get; set; } = true;

    public WorksheetItemList Traces { get; set; }

    public bool CalculateScene()
    {
        Log.L("Calculating scene");

        Router = new TwoDPathRouter(this, SheetSize, GridSize);
        Router.SetItems(Items, Nets);
        Traces = Router.GetTraces();

        if (SceneManager == null)
        {
            Log.L("Creating Manager");
            SceneManager = new DefaultSceneManager();
        }

        SceneManager.ShowGrid = ShowGrid;
        SceneManager.BackgroundColor = BackgroundColor;
        SceneManager.ForegroundColor = ForegroundColor;
        SceneManager.BackgroundHighColor = BackgroundHighColor;
        if (SceneManager.SetScene(GetDrawableComponents(), GetSelectedComponents(), SelectedPin))
        {
            SceneManager.DisplayOffset = DisplayOffset;

            SceneManager.SetSizeAndScale(SheetSize, GridSize);
            Log.L("Getting backend scene");
            object? backendScene = SceneManager.GetSceneForBackend();
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
            foreach (var pin in item.Pins)
            {
                pin.Size = new(1, 1, 0);
            }
            SelectedItems.Remove(item);
        }
    }

    public WorksheetItem DuplicateItem(WorksheetItem item)
    {
        WorksheetItem? newItem = item.Clone<WorksheetItem>(CloneOptions.DisableIgnoreAttributes);
        return newItem;
    }

    public DrawableComponentList GetDrawableComponents()
    {
        var list = new DrawableComponentList(this);

        foreach (var item in

                 Items
                     .Select(item =>
                         item.DrawableComponent
                     )
                     .Union(
                         Traces
                             .Select(item =>
                                 item.DrawableComponent
                             )
                     ))
        {
            list.Add(item);
        }
        return list;
    }

    public WorksheetItem? GetItemAt(float x, float y)
    {
        int iX = Convert.ToInt32(Math.Round(x));
        int iY = Convert.ToInt32(Math.Round(y));

        var selectedItem = Items.FirstOrDefault(
            item => item.X == iX && item.Y == iY);

        return (WorksheetItem?)selectedItem;
    }

    public bool IsSelected(WorksheetItem selectedItem)
    {
        return SelectedItems.Contains(selectedItem);
    }

    public void SelectItem(WorksheetItem item)
    {
        if (!SelectedItems.Contains(item))
        {
            foreach (var pin in item.Pins)
            {
                pin.Size = new(10, 10, 0);
            }

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
        var list = new DrawableComponentList(this);

        foreach (var item in SelectedItems.Select(item => item.DrawableComponent))
        {
            list.Add(item);
        }
        return list;
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