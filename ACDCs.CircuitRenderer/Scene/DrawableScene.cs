using System;
using System.Linq;
using ACDCs.CircuitRenderer.Definitions;
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
    public float Zoom => Workbook.Zoom;

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
                Scene.BackgroundColor.A
            )
            : Colors.WhiteSmoke;

        BackgroundColor = Scene?.BackgroundColor;
        ForegroundColor = Scene?.ForegroundColor ?? new Color(255, 255, 255);
        BackgroundHighColor = Scene?.BackgroundHighColor ?? new Color(70, 70, 70);

        canvas.FillRectangle(0, 0, 10000, 10000);
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

    public float GetScale(float size, float scale)
    {
        return size * scale;
    }

    public void Render(ICanvas canvas, IDrawableComponent drawable)
    {
        canvas.StrokeSize = BaseGridSize / 2;
        GetScaleAndZoom(drawable, out var drawPos, out var drawSize);

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

        foreach (var instruction in drawable.DrawInstructions)
        {
            if (instruction is LineInstruction line)
            {
                Log.L("line");
                SetStrokeColor(canvas, ForegroundColor ?? instruction.StrokeColor);
                DrawLine(canvas, drawSize, line);
            }

            if (instruction is BoxInstruction box)
            {
                Log.L("box");
                var upperLeft = new Coordinate(box.Position);
                var lowerRight = new Coordinate(box.Size);
                SetStrokeColor(canvas, ForegroundColor ?? box.StrokeColor);
                SetFillColor(canvas, box.FillColor);
                DrawRectangle(canvas, drawPos, drawSize, upperLeft, lowerRight, BackgroundHighColor ?? box.FillColor);
            }

            if (instruction is TextInstruction text)
            {
                Log.L("text");
                var centerPos = new Coordinate(instruction.Position);
                SetStrokeColor(canvas, ForegroundColor ?? instruction.StrokeColor);
                canvas.FontColor = ForegroundColor != null ? ForegroundColor.ToMauiColor() : new Microsoft.Maui.Graphics.Color(0, 0, 0);
                DrawText(canvas, drawPos, drawSize, text, centerPos, instruction);
            }

            if (instruction is CircleInstruction)
            {
                Log.L("circle");
                var centerPos = new Coordinate(instruction.Position);
                SetStrokeColor(canvas, ForegroundColor ?? instruction.StrokeColor);
                DrawCircle(canvas, centerPos, drawPos, drawSize);
            }

            if (instruction is PathInstruction path)
            {
                Log.L("path");
                SetStrokeColor(canvas, ForegroundColor ?? instruction.StrokeColor);
                DrawPath(canvas, drawPos, drawSize, path);
            }

            if (instruction is CurveInstruction curve)
            {
                Log.L("curve");
                SetStrokeColor(canvas, ForegroundColor ?? curve.StrokeColor);
                DrawCurve(canvas, drawPos, drawSize, curve.Position, curve.End, curve.AngleStart, curve.AngleEnd);
            }
        }

        foreach (var pin in drawable.DrawablePins)
        {
            canvas.SaveState();
            Log.L("pin");
            var posCenter = new Coordinate(pin.Position);
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
            var upperLeft = new Coordinate(-0.15f, -0.15f);
            SetStrokeColor(canvas, new Color(255, 100, 30));
            canvas.StrokeSize = 2;
            var lowerRight = new Coordinate(1.3f, 1.3f);
            DrawRectangle(canvas, drawPos, drawSize, upperLeft, lowerRight);
            canvas.RestoreState();
        }

        canvas.RestoreState();
    }

    public void SetFillColor(ICanvas canvas, Color fillColor)
    {
        canvas.FillColor = new Microsoft.Maui.Graphics.Color(fillColor.R, fillColor.G, fillColor.B);
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

    public void SetStrokeColor(ICanvas canvas, Color? penColor)
    {
        if (penColor != null)
        {
            canvas.StrokeColor = new Microsoft.Maui.Graphics.Color(penColor.R, penColor.G, penColor.B);
        }
    }

    private int _fontSize;

    private void DrawCircle(ICanvas canvas, Coordinate centerPos, Coordinate drawPos, Coordinate drawSize)
    {
        float x = GetScale(drawSize.X, centerPos.X);
        float y = GetScale(drawSize.Y, centerPos.Y);
        canvas.DrawCircle(x, y, Zoom * BaseGridSize * 0.1f);
    }

    private void DrawCurve(ICanvas canvas, Coordinate drawPos, Coordinate drawSize, Coordinate curvePosition,
                                Coordinate curveEnd, float curveAngleStart, float curveAngleEnd)
    {
        float startX = GetScale(drawSize.X, curvePosition.X);
        float startY = GetScale(drawSize.Y, curvePosition.Y);
        float width = GetScale(drawSize.X, curveEnd.X) - startX;
        float height = GetScale(drawSize.Y, curveEnd.Y) - startY;

        canvas.DrawArc(
            startX,
            startY,
            width,
            height,
            curveAngleStart,
            curveAngleEnd, false, false);
    }

    private void DrawLine(ICanvas canvas, Coordinate drawSize, LineInstruction line)
    {
        canvas.DrawLine(
            GetScale(drawSize.X, line.Position.X),
            GetScale(drawSize.Y, line.Position.Y),
            GetScale(drawSize.X, line.End.X),
            GetScale(drawSize.Y, line.End.Y));
    }

    private void DrawPath(ICanvas canvas, Coordinate drawPos, Coordinate drawSize, PathInstruction path)
    {
        PathF pathF = new();

        float scaleX = drawSize.X / path.Width;
        float scaleY = drawSize.Y / path.Height / 2;
        foreach (var part in path.GetParts())
            switch (part.Type)
            {
                case PathPartType.A:
                    break;

                case PathPartType.C:
                    {
                        pathF.CurveTo(
                            part.Coordinates[0].X * scaleX,
                            part.Coordinates[0].Y * scaleY,
                            part.Coordinates[1].X * scaleX,
                            part.Coordinates[1].Y * scaleY,
                            part.Coordinates[2].X * scaleX,
                            part.Coordinates[2].Y * scaleY
                        );
                    }
                    break;

                case PathPartType.M:
                    {
                        pathF.MoveTo(
                            part.Coordinates[0].X * scaleX,
                            part.Coordinates[0].Y * scaleY
                        );
                    }
                    break;

                case PathPartType.L:
                    {
                        pathF.LineTo(
                            part.Coordinates[0].X * scaleX,
                            part.Coordinates[0].Y * scaleY
                        );
                    }
                    break;

                case PathPartType.H:
                    break;

                case PathPartType.V:
                    break;

                case PathPartType.S:
                    break;

                case PathPartType.Q:
                    break;

                case PathPartType.T:
                    break;

                case PathPartType.Z:
                    break;
            }

        canvas.DrawPath(pathF);
    }

    private void DrawRectangle(ICanvas canvas, Coordinate drawPos, Coordinate drawSize, Coordinate upperLeft,
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

    private void DrawText(ICanvas canvas, Coordinate drawPos, Coordinate drawSize, TextInstruction text,
        Coordinate centerPos, IDrawInstruction instruction)
    {
        float x = GetScale(drawSize.X, centerPos.X);
        float y = GetScale(drawSize.Y, centerPos.Y);
        canvas.SaveState();

        if (text.IsRealFontSize)
        {
            canvas.FontSize = text.FontSize;
        }
        else
        {
            canvas.FontSize = (_fontSize / text.FontSize) * 12;
        }


        canvas.Translate(x, y);
        canvas.Rotate(text.Orientation);
        canvas.DrawString(text.Text, 0, 0, HorizontalAlignment.Center);
        canvas.RestoreState();
    }

    private void GetScaleAndZoom(IDrawableComponent drawable, out Coordinate drawPos, out Coordinate drawSize)
    {
        drawPos = new Coordinate(drawable.Position);
        drawSize = new Coordinate(drawable.Size);
        float offX = (drawSize.X - 2) / 2 * (Zoom * BaseGridSize);
        float offY = (drawSize.Y - 2) / 2 * (Zoom * BaseGridSize);
        drawSize.X = drawSize.X * Zoom * BaseGridSize;
        drawSize.Y = drawSize.Y * Zoom * BaseGridSize;
        drawPos.X = drawPos.X * Zoom * BaseGridSize;
        drawPos.Y = drawPos.Y * Zoom * BaseGridSize;
    }
}
