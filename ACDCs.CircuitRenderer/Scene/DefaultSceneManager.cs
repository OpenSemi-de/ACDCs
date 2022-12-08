﻿using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;

namespace ACDCs.CircuitRenderer.Scene;

public sealed class DefaultSceneManager : ISceneManager
{
    public Color? BackgroundColor { get; set; }
    public Color? BackgroundHighColor { get; set; }
    public Coordinate? DisplayOffset { get; set; }
    public object? DrawableScene { get; set; }
    public Color? ForegroundColor { get; set; }
    public SheetScene? Scene { get; set; }
    public bool ShowGrid { get; set; } = true;

    public object? GetSceneForBackend()
    {
        DrawableScene = new DrawableScene(Scene)
        {
            DisplayOffset = DisplayOffset
        };

        return DrawableScene;
    }

    public bool SendToBackend(object? backendScene)
    {
        return true;
    }

    public bool SetScene(DrawableComponentList drawables, DrawableComponentList selected, PinDrawable? selectedPin)
    {
        Scene = new SheetScene();
        Scene.SetDrawables(drawables, selected);
        Scene.ShowGrid = ShowGrid;
        Scene.BackgroundColor = BackgroundColor;
        Scene.ForegroundColor = ForegroundColor;
        Scene.BackgroundHighColor = BackgroundHighColor;
        Scene.DisplayOffset = DisplayOffset;
        Scene.SelectedPin = selectedPin;
        return true;
    }

    public void SetSizeAndScale(Coordinate sheetSize, float gridSize)
    {
        if (Scene != null)
        {
            Scene.GridSize = gridSize;
            Scene.SheetSize = sheetSize;
        }
    }
}