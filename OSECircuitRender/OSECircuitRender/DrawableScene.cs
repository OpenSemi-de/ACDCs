using Microsoft.Maui.Graphics;
using System;
using System.Linq;

namespace OSECircuitRender
{
    public class DrawableScene : IDrawable
    {
        public static float BaseGridSize = 2.54f;
        public static float Zoom = 10f;
        public SheetScene Scene { get; }
        private ICanvas Canvas { get; set; }

        public DrawableScene(SheetScene scene)
        {
            Scene = scene;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Canvas = canvas;
            Canvas.FillColor = Colors.BlanchedAlmond;
            Canvas.FillRectangle(0, 0, 10000, 10000);
            if (Scene == null) return;
            Scene.Drawables.ForEach(
                drawable =>
                {
                    Render(drawable);
                }
                );
        }

        public void Render(IDrawableComponent drawable)
        {
            Canvas.StrokeSize = BaseGridSize / 2;
            GetScaleAndZoom(drawable, out DrawCoordinate drawPos, out DrawCoordinate drawSize);

            foreach (var instruction in drawable.DrawInstructions)
            {
                if (instruction is Line)
                {
                    Log.L("line");
                    var start = instruction.Coordinates[0];
                    var end = instruction.Coordinates[1];
                    SetStrokeColor(Canvas, instruction.Colors[0]);
                    Canvas.DrawLine(
                        GetAbs(drawPos.x, drawSize.x, start.x),
                        GetAbs(drawPos.y, drawSize.y, start.y),
                        GetAbs(drawPos.x, drawSize.x, end.x),
                        GetAbs(drawPos.y, drawSize.y, end.y));
                }

                if (instruction is Box)
                {
                    Log.L("box");
                    var upperLeft = instruction.Coordinates[0];
                    var lowerRight = instruction.Coordinates[1];
                    SetStrokeColor(Canvas, instruction.Colors[0]);
                    SetFillColor(Canvas, instruction.Colors[1]);
                    Canvas.DrawRectangle(
                        GetAbs(drawPos.x, drawSize.x, upperLeft.x),
                        GetAbs(drawPos.y, drawSize.y, upperLeft.y),
                        GetScale(drawSize.x, lowerRight.x),
                        GetScale(drawSize.y, lowerRight.y));
                }

                if (instruction is Text text)
                {
                    Log.L("text");
                    var centerPos = instruction.Coordinates[0];
                    var x = GetAbs(drawPos.x, drawSize.x, centerPos.x);
                    var y = GetAbs(drawPos.y, drawSize.y, centerPos.y);
                    Canvas.SaveState();
                    Canvas.FontSize = text.size;
                    Canvas.Translate(x, y);
                    Canvas.Rotate(text.orientation);
                    SetStrokeColor(Canvas, instruction.Colors[0]);
                    Canvas.DrawString(text.text, 0, 0, HorizontalAlignment.Center);
                    Canvas.Translate(0, 0);
                    Canvas.ResetState();
                }

                if (instruction is Circle)
                {
                    Log.L("circle");
                    var posCenter = instruction.Coordinates[0];
                    posCenter.x = GetAbs(drawPos.x, drawSize.x, posCenter.x);
                    posCenter.y = GetAbs(drawPos.y, drawSize.y, posCenter.y);
                    SetStrokeColor(Canvas, instruction.Colors[0]);
                    Canvas.DrawCircle(posCenter.x, posCenter.y, Zoom * BaseGridSize * 0.1f);
                }

                if (instruction is Path path)
                {
                    SetStrokeColor(Canvas, instruction.Colors[0]);
                    Path scenePath = path;

                    PathF pathF = new();
                    Canvas.DrawPath(pathF);

                    //path.Points.Append(PointF)
                    //instruction.Coordinates
                }
            }

            foreach (var pin in drawable.DrawablePins)
            {
                Log.L("pin");
                var posCenter = pin.Position;
                posCenter.x = GetAbs(drawPos.x, drawSize.x, posCenter.x);
                posCenter.y = GetAbs(drawPos.y, drawSize.y, posCenter.y);
                SetStrokeColor(Canvas, pin.DrawInstructions[0].Colors[0]);
                Canvas.DrawCircle(posCenter.x, posCenter.y, Zoom * BaseGridSize * 0.1f);
            }

            //	IImage image = ((PictureCanvas)Canvas).Picture;
        }

        private static void GetScaleAndZoom(IDrawableComponent drawable, out DrawCoordinate drawPos, out DrawCoordinate drawSize)
        {
            drawPos = drawable.Position;
            drawSize = drawable.Size;
            drawSize.x = drawSize.x * Zoom * BaseGridSize;
            drawSize.y = drawSize.y * Zoom * BaseGridSize;
            drawPos.x = drawPos.x * Zoom * BaseGridSize;
            drawPos.y = drawPos.y * Zoom * BaseGridSize;
        }

        public static void SetFillColor(ICanvas canvas, Color fillColor)
        {
            canvas.StrokeColor = new Microsoft.Maui.Graphics.Color(fillColor.r, fillColor.g, fillColor.b);
        }

        public static void SetStrokeColor(ICanvas canvas, Color penColor)
        {
            canvas.StrokeColor = new Microsoft.Maui.Graphics.Color(penColor.r, penColor.g, penColor.b);
        }

        public static float GetAbs(float pos, float size, float scale)
        {
            return pos + (size * scale);
        }

        public static float GetScale(float size, float scale)
        {
            return (size * scale);
        }
    }
}