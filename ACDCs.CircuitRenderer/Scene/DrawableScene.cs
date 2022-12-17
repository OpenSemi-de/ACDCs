﻿using System;
using System.Linq;
using ACDCs.CircuitRenderer.Definitions;
using ACDCs.CircuitRenderer.Drawables;
using ACDCs.CircuitRenderer.Instructions;
using ACDCs.CircuitRenderer.Interfaces;
using Microsoft.Maui.Graphics;
using Color = ACDCs.CircuitRenderer.Definitions.Color;

namespace ACDCs.CircuitRenderer.Scene;

public class DrawableScene : IDrawable
{
    public DrawableScene(SheetScene? scene)
    {
        Scene = scene;
        SheetSize = scene?.SheetSize != null ? scene.SheetSize : new(100, 100, 0);
        SetScene(scene);
    }

    public Color? BackgroundColor { get; set; }
    public Color? BackgroundHighColor { get; set; }
    public float BaseGridSize => CurrentGridSize ?? Workbook.BaseGridSize;
    public float? CurrentGridSize { get; set; }
    public Coordinate? DisplayOffset { get; set; }
    public Color? ForegroundColor { get; set; }
    public bool IsRendering { get; private set; }
    public SheetScene? Scene { get; private set; }
    public Coordinate SheetSize { get; set; }
    private static float Zoom => Workbook.Zoom;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        IsRendering = true;
        _fontSize = Convert.ToInt32(Math.Round(BaseGridSize * Zoom / 2));
        canvas.Antialias = true;

        if (Workbook.BaseFontName != "")
        {
            canvas.Font = new Font(Workbook.BaseFontName);
        }

        canvas.FillColor = Scene?.BackgroundColor != null
            ? new Microsoft.Maui.Graphics.Color(
                Scene.BackgroundColor.R,
                Scene.BackgroundColor.G,
                Scene.BackgroundColor.B,
                Scene.BackgroundColor.A / 255
                                               )
            : Colors.WhiteSmoke;

        canvas.FillColor = Colors.Transparent;

        BackgroundColor = Scene?.BackgroundColor;
        ForegroundColor = Scene?.ForegroundColor ?? new Color(255, 255, 255);
        BackgroundHighColor = Scene?.BackgroundHighColor ?? new Color(70, 70, 70);

        canvas.SaveState();

        if (DisplayOffset != null)
        {
            canvas.Translate(DisplayOffset.X, DisplayOffset.Y);
        }

        canvas.StrokeSize = 0.5f;
        canvas.StrokeColor = ForegroundColor.ToMauiColor();
        if (Scene != null && Scene.ShowGrid)
        {
            for (float x = 0; x < BaseGridSize * Zoom * SheetSize.X; x += Zoom * BaseGridSize)
                canvas.DrawLine(x, 0, x, BaseGridSize * Zoom * SheetSize.Y);

            for (float y = 0; y < BaseGridSize * Zoom * SheetSize.Y; y += Zoom * BaseGridSize)
                canvas.DrawLine(0, y, BaseGridSize * Zoom * SheetSize.X, y);
        }

        canvas.RestoreState();

        

        Scene?.Drawables?.ForEach(
               component => Render(canvas, component)
                                 );
        IsRendering = false;
    }

    public static float GetScale(float size, float scale)
    {
        return size * scale;
    }

    public void Render(ICanvas canvas, IDrawableComponent drawable)
    {

        canvas.StrokeSize = BaseGridSize / 2;
        GetScaleAndZoom(drawable, out Coordinate drawPos, out Coordinate drawSize, Zoom, BaseGridSize);

        canvas.SaveState();

        if (DisplayOffset != null)
        {
            canvas.Translate(DisplayOffset.X, DisplayOffset.Y);
        }

        int lowestPinX = 0;
        int lowestPinY = 0;

        if (drawable.DrawablePins.Any())
        {
            lowestPinX = Convert.ToInt32(Math.Round(drawable.DrawablePins.Min(p => p.Position.X) * drawSize.X));
            lowestPinY = Convert.ToInt32(Math.Round(drawable.DrawablePins.Min(p => p.Position.Y) * drawSize.Y));
        }

        canvas.Translate(drawPos.X - lowestPinX, drawPos.Y - lowestPinY);

        canvas.Rotate(drawable.Rotation, drawSize.X / 2, drawSize.Y / 2);

        RenderInstruction renderInstruction = new()
        {
            Zoom = Zoom,
            BaseGridSize = Scene?.GridSize ?? BaseGridSize,
            ForegroundColor = Scene?.ForegroundColor ?? ForegroundColor,
            BackgroundColor = Scene?.BackgroundColor ?? BackgroundColor,
            DrawPos = drawPos,
            DrawSize = drawSize,
            FontSize = _fontSize,
        };

        foreach (IDrawInstruction instruction in drawable.DrawInstructions)
        {
            switch (instruction)
            {
                case LineInstruction line:
                    RenderManager.Render(canvas, renderInstruction, line);
                    break;
                case BoxInstruction box:
                    RenderManager.Render(canvas, renderInstruction, box);
                    break;
                case TextInstruction text:
                    RenderManager.Render(canvas, renderInstruction, text);
                    break;
                case CircleInstruction circle:
                    RenderManager.Render(canvas, renderInstruction, circle);
                    break;
                case PathInstruction path:
                    RenderManager.Render(canvas, renderInstruction, path);
                    break;
                case CurveInstruction curve:
                    RenderManager.Render(canvas, renderInstruction, curve);
                    break;
            }
        }

        foreach (PinDrawable pin in drawable.DrawablePins)
        {
            canvas.SaveState();
            Log.L("pin");
            Coordinate posCenter = new(pin.Position);
            posCenter.X = GetScale(drawSize.X, posCenter.X);
            posCenter.Y = GetScale(drawSize.Y, posCenter.Y);
            canvas.FillColor = BackgroundHighColor != null ? BackgroundHighColor.ToMauiColor() : new Microsoft.Maui.Graphics.Color(255, 255, 255);

            if (pin == Scene?.SelectedPin)
            {
                canvas.FillColor = Colors.OrangeRed;
            }

            SetStrokeColor(canvas, ForegroundColor ?? pin.DrawInstructions[0].StrokeColor);
            float selectedSize = Scene != null && Scene.IsSelected(drawable) ? 3f : 1f;
            canvas.FillCircle(posCenter.X, posCenter.Y, Zoom * BaseGridSize * 0.2f * selectedSize);

            if (pin.PinText != "")
            {
                canvas.SaveState();
                canvas.FontSize = Convert.ToSingle(_fontSize * 0.75 * (selectedSize / 2));
                canvas.FontColor = ForegroundColor != null ? ForegroundColor.ToMauiColor() : new Microsoft.Maui.Graphics.Color(0, 0, 0);
                canvas.FillColor = BackgroundHighColor != null ? BackgroundHighColor.ToMauiColor() : new Microsoft.Maui.Graphics.Color(255, 255, 255);

                if (pin == Scene?.SelectedPin)
                {
                    canvas.FillColor = Colors.OrangeRed;
                }
                canvas.FillCircle(posCenter.X, posCenter.Y, Zoom * BaseGridSize * 0.2f * selectedSize);

                canvas.DrawString(pin.PinText, posCenter.X, posCenter.Y + Zoom * BaseGridSize * 0.12f,
                    HorizontalAlignment.Center);
                canvas.RestoreState();
            }

            canvas.DrawCircle(posCenter.X, posCenter.Y, Zoom * BaseGridSize * 0.2f * selectedSize);
            canvas.RestoreState();
        }

        if (Scene != null && Scene.IsSelected(drawable))
        {
            canvas.SaveState();
            Log.L("selected");
            Coordinate upperLeft = new(-0.15f, -0.15f);
            SetStrokeColor(canvas, new Color(255, 100, 30));
            canvas.StrokeSize = 2;
            Coordinate lowerRight = new(1.3f, 1.3f);
            DrawRectangle(canvas, drawSize, upperLeft, lowerRight);
            canvas.RestoreState();
        }

        Scene?.SceneManager.PutFeedbackRect(Scene.IsSelected(drawable), drawable as DrawableComponent, drawPos,
            drawSize, DisplayOffset);

        canvas.RestoreState();
    }


    public static void SetFillColor(ICanvas canvas, Color? fillColor)
    {
        if (fillColor != null)
        {
            canvas.FillColor = new Microsoft.Maui.Graphics.Color(fillColor.R, fillColor.G, fillColor.B);
        }
    }

    public void SetScene(SheetScene? scene)
    {
        Scene = scene;
        if (scene != null && scene.GridSize != 0)
        {
            CurrentGridSize = scene.GridSize;
        }
        else
        {
            CurrentGridSize = Workbook.BaseGridSize;
        }

        if (scene?.SheetSize != null) SheetSize = scene.SheetSize;
    }

    public static void SetStrokeColor(ICanvas canvas, Color? penColor)
    {
        if (penColor != null)
        {
            canvas.StrokeColor = new Microsoft.Maui.Graphics.Color(penColor.R, penColor.G, penColor.B);
        }
    }

    private int _fontSize;


    private static void DrawRectangle(ICanvas canvas, Coordinate drawSize, Coordinate upperLeft,
        Coordinate lowerRight, Color? fillColor = null)
    {
        if (fillColor != null)
            canvas.FillRectangle(
                GetScale(drawSize.X, upperLeft.X),
                GetScale(drawSize.Y, upperLeft.Y),
                GetScale(drawSize.X, lowerRight.X),
                GetScale(drawSize.Y, lowerRight.Y));

        canvas.DrawRectangle(
            GetScale(drawSize.X, upperLeft.X),
            GetScale(drawSize.Y, upperLeft.Y),
            GetScale(drawSize.X, lowerRight.X),
            GetScale(drawSize.Y, lowerRight.Y));
    }


    private static void GetScaleAndZoom(IDrawableComponent drawable, out Coordinate drawPos, out Coordinate drawSize, float Zoom, float BaseGridSize)
    {
        drawPos = new Coordinate(drawable.Position);
        drawSize = new Coordinate(drawable.Size);
        drawSize.X = drawSize.X * Zoom * BaseGridSize;
        drawSize.Y = drawSize.Y * Zoom * BaseGridSize;
        drawPos.X = drawPos.X * Zoom * BaseGridSize;
        drawPos.Y = drawPos.Y * Zoom * BaseGridSize;
    }
}
