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
using Color = ACDCs.CircuitRenderer.Definitions.Color;

namespace ACDCs.CircuitRenderer.Sheet;

using System.Collections.Generic;
using Instructions;

public sealed class Worksheet
{
    public Color? BackgroundColor { get; set; }

    public Color? BackgroundHighColor { get; set; }

    public string Directory { get; set; } = string.Empty;
    public Coordinate? DisplayOffset { get; set; }

    public string Filename { get; set; } = string.Empty;
    public Color? ForegroundColor { get; set; }

    public float GridSize { get; set; } = 2.54f;

    public bool IsMultiselectionEnabled { get; set; }

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

    [JsonIgnore]
    public WorksheetItemList Traces { get; set; }

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

    public void AddRoute(PinDrawable pinFrom, PinDrawable pinTo)
    {
        NetItem? netFromPin = Nets.Cast<NetItem>().FirstOrDefault(net => net.Pins.Any(pin => pin.Equals(pinFrom.ComponentGuid)));
        NetItem? netToPin = Nets.Cast<NetItem>().FirstOrDefault(net => net.Pins.Any(pin => pin.Equals(pinTo.ComponentGuid)));
        if (netToPin == null & netFromPin == null)
        {
            Nets.AddNet(pinFrom, pinTo);
        }

        if (netToPin == null && netFromPin != null)
        {
            if (!netFromPin.Pins.Any(pin => pin.Equals(pinTo.ComponentGuid)))
                netFromPin.Pins.Add(pinTo.ComponentGuid);
        }

        if (netToPin != null && netFromPin == null)
        {
            if (!netToPin.Pins.Any(pin => pin.Equals(pinTo.ComponentGuid)))
                netToPin.Pins.Add(pinTo.ComponentGuid);
        }

        StartRouter();

        SelectedPin = null;
    }

    public bool CalculateScene()
    {
        Log.L("Calculating scene");

        if (SceneManager == null)
        {
            Log.L("Creating Manager");
            SceneManager = new DefaultSceneManager();
        }

        SceneManager.ShowGrid = ShowGrid;
        SceneManager.BackgroundColor = BackgroundColor;
        SceneManager.ForegroundColor = ForegroundColor;
        SceneManager.BackgroundHighColor = BackgroundHighColor;
        if (!SceneManager.SetScene(GetDrawableComponents(), GetSelectedComponents(), SelectedPin))
        {
            return false;
        }

        SceneManager.DisplayOffset = DisplayOffset;

        SceneManager.SetSizeAndScale(SheetSize, GridSize);
        Log.L("Getting backend scene");
        object? backendScene = SceneManager.GetSceneForBackend();
        if (!SceneManager.SendToBackend(backendScene))
        {
            return false;
        }

        Log.L("Backend received scene");

        return true;
    }

    public void DeleteItem(WorksheetItem item)
    {
        if (SelectedItems.Contains(item))
            SelectedItems.Remove(item);

        if (Items.Contains(item))
            Items.Remove(item);

        StartRouter();
    }

    public void DeleteTrace(TraceItem trace, LineInstruction? selectedTraceLine = null)
    {
        IWorksheetItem? traceNet = Nets.FirstOrDefault(iitem => (NetItem)iitem == trace.Net);

        if (selectedTraceLine != null)
        {
            if (trace.DrawableComponent is TraceDrawable traceDrawable)
            {
                PinDrawable? pinFrom = traceDrawable.GetPinFrom(selectedTraceLine);
                PinDrawable? pinTo = traceDrawable.GetPinTo(selectedTraceLine);
                int usedSubnetsFrom = traceDrawable.CountSubnets(pinFrom);
                int usedSubnetsTo = traceDrawable.CountSubnets(pinTo);

                // if (traceNet != null && pinTo != null && traceNet.Pins.Contains(pinTo))
                // {
                //     traceNet.Pins.Remove(pinTo);
                // }

                if (traceNet != null && pinFrom != null)
                {
                    if (usedSubnetsFrom == 1 && traceNet.Pins.Contains(pinFrom))
                    {
                        traceNet.Pins.Remove(pinFrom);
                    }
                    else if (usedSubnetsTo == 1 && traceNet.Pins.Contains(pinTo))
                    {
                        traceNet.Pins.Remove(pinTo);
                    }
                }
            }
            StartRouter();
            return;
        }

        DeleteItem(trace);
        if (Traces.Contains(trace))
        {
            Traces.Remove(trace);
        }

        if (traceNet != null)
        {
            Nets.Remove(traceNet);
        }

        StartRouter();
    }

    public void DeselectItem(WorksheetItem item)
    {
        if (!SelectedItems.Contains(item))
        {
            return;
        }

        foreach (PinDrawable pin in item.Pins)
        {
            pin.Size = new Coordinate(1, 1, 0);
        }
        SelectedItems.Remove(item);
    }

    public WorksheetItem DuplicateItem(WorksheetItem item)
    {
        WorksheetItem? newItem = item.Clone(CloneOptions.DisableIgnoreAttributes);
        StartRouter();
        return newItem;
    }

    public DrawableComponentList GetDrawableComponents()
    {
        DrawableComponentList list = new(this);

        IEnumerable<IDrawableComponent> drawableComponents = Items.AsParallel()
            .Select(item =>
                item.DrawableComponent
            )
            .Union(
                Traces.AsParallel()
                    .Select(item =>
                        item.DrawableComponent
                    )
            );

        foreach (IDrawableComponent item in drawableComponents)
        {
            list.Add(item);
        }
        return list;
    }

    public WorksheetItem? GetItemAt(float x, float y)
    {
        int iX = Convert.ToInt32(Math.Round(x));
        int iY = Convert.ToInt32(Math.Round(y));

        IWorksheetItem? selectedItem = Items.FirstOrDefault(
            item => item.X == iX && item.Y == iY);

        return (WorksheetItem?)selectedItem;
    }

    public bool IsSelected(WorksheetItem selectedItem)
    {
        return SelectedItems.Contains(selectedItem);
    }

    public void MirrorItem(WorksheetItem item)
    {
        float centerX = item.Width / 2;
        item.DrawableComponent.DrawInstructions
            .ForEach(instruction => MirrorInstruction(centerX, instruction));
        item.IsMirrored = !item.IsMirrored;
        item.DrawableComponent.IsMirroringDone = true;
        StartRouter();
    }

    public void RotateItem(WorksheetItem item)
    {
        item.Rotation += 90;
        if (item.Rotation >= 360)
            item.Rotation = 0;
        StartRouter();
    }

    public void SelectItem(WorksheetItem item)
    {
        if (SelectedItems.Contains(item))
        {
            return;
        }

        foreach (PinDrawable pin in item.Pins)
        {
            pin.Size = new Coordinate(10, 10, 0);
        }

        SelectedItems.AddItem(item);
    }

    public void StartRouter()
    {
        Router.SetItems(Items, Nets);
        Traces.Clear();
        Traces.AddRange(Router.GetTraces());
        if (SceneManager != null)
        {
            SceneManager.DebugRects = Router.DebugRects;
            SceneManager.CollisionMap = Router.CollisionMap;
        }
    }

    public bool ToggleSelectItem(WorksheetItem selectedItem)
    {
        if (IsMultiselectionEnabled)
        {
            if (SelectedItems.Contains(selectedItem))
            {
                SelectedItems.Remove(selectedItem);
                return false;
            }

            SelectedItems.Add(selectedItem);
            return true;
        }

        SelectedItems.Clear();
        SelectedItems.Add(selectedItem);
        return true;
    }

    public void UseMultiselect(bool enabled)
    {
        IsMultiselectionEnabled = enabled;

        if (enabled)
        {
            SelectedItems.Clear();
        }
    }

    private static void MirrorInstruction(float centerX, IDrawInstruction instruction)
    {
        instruction.Coordinates.ForEach(coordinate =>
            {
                if (coordinate.X > centerX)
                {
                    coordinate.X = centerX - (coordinate.X - centerX);
                }
                else
                {
                    coordinate.X = centerX + (centerX - coordinate.X);
                }
            });
    }

    private DrawableComponentList GetSelectedComponents()
    {
        DrawableComponentList list = new(this);

        foreach (IDrawableComponent item in SelectedItems.AsParallel().Select(item => item.DrawableComponent).ToList())
        {
            list.Add(item);
        }
        return list;
    }

    private void OnItemAdded(IWorksheetItem item)
    {
        StartRouter();
    }

    private void OnSelectionAdded(IWorksheetItem obj)
    {
        OnSelectionChange?.Invoke(SelectedItems);
    }
}
