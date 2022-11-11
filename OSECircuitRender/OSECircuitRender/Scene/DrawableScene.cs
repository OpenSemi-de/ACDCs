using System;
using System.Linq;
using Microsoft.Maui.Graphics;
using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;
using Color = OSECircuitRender.Definitions.Color;

namespace OSECircuitRender.Scene
{
    public class DrawableScene : IDrawable
    {
        public static float BaseGridSize = 2.54f;
        public static float Zoom = 10f;
        public SheetScene Scene { get; }

        public DrawableScene(SheetScene scene)
        {
            Scene = scene;
            if (scene.GridSize != 0)
                BaseGridSize = scene.GridSize;
            SheetSize = scene.SheetSize;
        }

        public Coordinate SheetSize { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.WhiteSmoke;
            canvas.FillRectangle(0, 0, 10000, 10000);
            if (Scene == null) return;
            canvas.SaveState();
            canvas.StrokeSize = 0.2f;
            canvas.StrokeColor = Colors.SlateGray;
            if (Scene.ShowGrid)
            {
                for (float x = 0; x < BaseGridSize * Zoom * SheetSize.X; x = x + Zoom * BaseGridSize)
                {
                    canvas.DrawLine(x, 0, x, 10000);
                    canvas.DrawLine(0, x, 10000, x);
                }
            }

            canvas.RestoreState();
            Scene.Drawables.ForEach(
                component => Render(canvas, component)
            );
        }

        public void Render(ICanvas canvas, IDrawableComponent drawable)
        {
            canvas.StrokeSize = BaseGridSize / 2;
            GetScaleAndZoom(drawable, out Coordinate drawPos, out Coordinate drawSize);

            canvas.SaveState();

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
                    SetStrokeColor(canvas, instruction.StrokeColor);
                    DrawLine(canvas, drawSize, line);
                }

                if (instruction is BoxInstruction box)
                {
                    Log.L("box");
                    var upperLeft = new Coordinate(box.Position);
                    var lowerRight = new Coordinate(box.Size);
                    SetStrokeColor(canvas, box.StrokeColor);
                    SetFillColor(canvas, box.FillColor);
                    DrawRectangle(canvas, drawPos, drawSize, upperLeft, lowerRight, box.FillColor);
                }

                if (instruction is TextInstruction text)
                {
                    Log.L("text");
                    var centerPos = new Coordinate(instruction.Position);
                    SetStrokeColor(canvas, instruction.StrokeColor);
                    DrawText(canvas, drawPos, drawSize, text, centerPos, instruction);
                }

                if (instruction is CircleInstruction)
                {
                    Log.L("circle");
                    var centerPos = new Coordinate(instruction.Position);
                    SetStrokeColor(canvas, instruction.StrokeColor);
                    DrawCircle(canvas, centerPos, drawPos, drawSize);
                }

                if (instruction is PathInstruction path)
                {
                    Log.L("path");

                    SetStrokeColor(canvas, instruction.StrokeColor);

                    DrawPath(canvas, drawPos, drawSize, path);
                }

                if (instruction is CurveInstruction curve)
                {
                    Log.L("curve");
                    SetStrokeColor(canvas, curve.StrokeColor);
                    DrawCurve(canvas, drawPos, drawSize, curve.Position, curve.End, curve.AngleStart, curve.AngleEnd);
                }
            }

            foreach (var pin in drawable.DrawablePins)
            {
                Log.L("pin");
                var posCenter = new Coordinate(pin.Position);
                posCenter.X = GetScale(drawSize.X, posCenter.X);
                posCenter.Y = GetScale(drawSize.Y, posCenter.Y);
                //canvas.Translate(posCenter.X, posCenter.Y);
                SetStrokeColor(canvas, pin.DrawInstructions[0].StrokeColor);
                canvas.FillCircle(posCenter.X, posCenter.Y, Zoom * BaseGridSize * 0.2f);

                if (pin.PinText != "")
                {
                    canvas.SaveState();
                    canvas.FontSize = 9;
                    canvas.FillColor = Colors.White;
                    canvas.FillCircle(posCenter.X, posCenter.Y, Zoom * BaseGridSize * 0.2f);

                    canvas.DrawString(pin.PinText, posCenter.X, posCenter.Y + Zoom * BaseGridSize * 0.12f, HorizontalAlignment.Center);
                    canvas.RestoreState();
                }
                canvas.DrawCircle(posCenter.X, posCenter.Y, Zoom * BaseGridSize * 0.2f);
            }

            canvas.RestoreState();
        }

        private void DrawCurve(ICanvas canvas, Coordinate drawPos, Coordinate drawSize, Coordinate curvePosition,
            Coordinate curveEnd, float curveAngleStart, float curveAngleEnd)
        {
            var startX = GetScale(drawSize.X, curvePosition.X);
            var startY = GetScale(drawSize.Y, curvePosition.Y);
            var width = GetScale(drawSize.X, curveEnd.X) - startX;
            var height = GetScale(drawSize.Y, curveEnd.Y) - startY;

            // canvas.DrawRectangle(startX, startY, width, height);
            canvas.DrawArc(
                x: startX,
                y: startY,
                width: width,
                height: height,
                startAngle: curveAngleStart,
                endAngle: curveAngleEnd, false, false);
        }

        private static void DrawPath(ICanvas canvas, Coordinate drawPos, Coordinate drawSize, PathInstruction path)
        {
            PathF pathF = new();

            float scaleX = drawSize.X / path.Width;
            float scaleY = drawSize.Y / path.Height / 2;
            foreach (var part in path.GetParts())
            {
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

                    default:
                        break;
                }
            }

            canvas.DrawPath(pathF);
        }

        private static void DrawCircle(ICanvas canvas, Coordinate centerPos, Coordinate drawPos, Coordinate drawSize)
        {
            var x = GetScale(drawSize.X, centerPos.X);
            var y = GetScale(drawSize.Y, centerPos.Y);
            canvas.DrawCircle(x, y, Zoom * BaseGridSize * 0.1f);
        }

        private static void DrawText(ICanvas canvas, Coordinate drawPos, Coordinate drawSize, TextInstruction text,
            Coordinate centerPos, IDrawInstruction instruction)
        {
            var x = GetScale(drawSize.X, centerPos.X);
            var y = GetScale(drawSize.Y, centerPos.Y);
            canvas.SaveState();
            canvas.FontSize = text.Size;
            canvas.Translate(x, y);
            canvas.Rotate(text.Orientation);
            canvas.DrawString(text.Text, 0, 0, HorizontalAlignment.Center);
            canvas.RestoreState();
        }

        private static void DrawRectangle(ICanvas canvas, Coordinate drawPos, Coordinate drawSize, Coordinate upperLeft,
            Coordinate lowerRight, Color fillColor = null)
        {
            if (fillColor != null)
            {
                canvas.FillRectangle(
                    GetScale(drawSize.X, upperLeft.X),
                    GetScale(drawSize.Y, upperLeft.Y),
                    GetScale(drawSize.X, lowerRight.X),
                    GetScale(drawSize.Y, lowerRight.Y));
            }

            canvas.DrawRectangle(
                    GetScale(drawSize.X, upperLeft.X),
                    GetScale(drawSize.Y, upperLeft.Y),
                    GetScale(drawSize.X, lowerRight.X),
                    GetScale(drawSize.Y, lowerRight.Y));
        }

        private static void DrawLine(ICanvas canvas, Coordinate drawSize, LineInstruction line)
        {
            canvas.DrawLine(
                GetScale(drawSize.X, line.Position.X),
                GetScale(drawSize.Y, line.Position.Y),
                GetScale(drawSize.X, line.End.X),
                GetScale(drawSize.Y, line.End.Y));
        }

        private static void GetScaleAndZoom(IDrawableComponent drawable, out Coordinate drawPos, out Coordinate drawSize)
        {
            drawPos = new Coordinate(drawable.Position);
            drawSize = new Coordinate(drawable.Size);
            var offX = (drawSize.X - 2) / 2 * (Zoom * BaseGridSize);
            var offY = (drawSize.Y - 2) / 2 * (Zoom * BaseGridSize);
            drawSize.X = drawSize.X * Zoom * BaseGridSize;
            drawSize.Y = drawSize.Y * Zoom * BaseGridSize;
            drawPos.X = drawPos.X * Zoom * BaseGridSize;
            drawPos.Y = drawPos.Y * Zoom * BaseGridSize;
            //drawPos.X += offX;
            //  drawPos.Y += offY;
            //  drawPos.X = drawPos.X - Zoom * BaseGridSize * ((drawable.Size.X % 2) / 2);
            //  drawPos.Y = drawPos.Y - Zoom * BaseGridSize * ((drawable.Size.Y % 2) / 2);
        }

        public static void SetFillColor(ICanvas canvas, Definitions.Color fillColor)
        {
            canvas.FillColor = new Microsoft.Maui.Graphics.Color(fillColor.R, fillColor.G, fillColor.B);
        }

        public static void SetStrokeColor(ICanvas canvas, Definitions.Color penColor)
        {
            canvas.StrokeColor = new Microsoft.Maui.Graphics.Color(penColor.R, penColor.G, penColor.B);
        }

        public static float GetScale(float size, float scale)
        {
            return (size * scale);
        }
    }
}