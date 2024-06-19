using System.Collections.Generic;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Interfaces;
using Microsoft.Maui.Graphics;
using Color = ACDCs.CircuitRenderer.Definitions.Color;

namespace ACDCs.CircuitRenderer.Scene;

public sealed class DefaultSceneManager : ISceneManager
{
    public Color? BackgroundColor { get; set; }
    public Color? BackgroundHighColor { get; set; }
    public short[,] CollisionMap { get; set; } = new short[0, 0];
    public List<RectFr> DebugRects { get; set; } = new();
    public Coordinate? DisplayOffset { get; set; }
    public object? DrawableScene { get; set; }
    public List<FeedbackRect>? FeedbackRects { get; } = new();
    public Color? ForegroundColor { get; set; }
    public SheetScene? Scene { get; set; }
    public bool ShowCollisionMap { get; set; }
    public bool ShowGrid { get; set; } = true;

    public object? GetSceneForBackend()
    {
        DrawableScene = new DrawableScene(Scene)
        {
            DisplayOffset = DisplayOffset
        };

        return DrawableScene;
    }

    public void PutFeedbackRect(bool isSelected, DrawableComponent? drawable, Coordinate drawPos, Coordinate drawSize,
        Coordinate? displayOffset)
    {
        FeedbackRect feedBackRect = new(isSelected, drawable)
        {
            Rect = new RectF(drawPos.Substract(displayOffset).ToPointF(),
            drawPos.Add(drawSize).Substract(displayOffset).ToSizeF())
        };

        FeedbackRects?.Add(feedBackRect);
    }

    public bool SendToBackend(object? backendScene)
    {
        return true;
    }

    public bool SetScene(DrawableComponentList drawables, DrawableComponentList selected, PinDrawable? selectedPin)
    {
        Scene = new SheetScene(this);
        Scene.SetDrawables(drawables, selected);
        Scene.ShowGrid = ShowGrid;
        Scene.BackgroundColor = BackgroundColor;
        Scene.ForegroundColor = ForegroundColor;
        Scene.BackgroundHighColor = BackgroundHighColor;
        Scene.DisplayOffset = DisplayOffset;
        Scene.SelectedPin = selectedPin;
        Scene.DebugRects = DebugRects;
        Scene.CollisionMap = CollisionMap;
        Scene.ShowCollisionMap = ShowCollisionMap;
        return true;
    }

    public void SetSizeAndScale(Coordinate sheetSize, float gridSize)
    {
        if (Scene == null)
        {
            return;
        }

        Scene.GridSize = gridSize;
        Scene.SheetSize = sheetSize;
    }
}
