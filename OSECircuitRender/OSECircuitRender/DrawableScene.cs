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

        public DrawableScene(SheetScene scene)
        {
            Scene = scene;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.BlanchedAlmond;
            canvas.FillRectangle(0, 0, 10000, 10000);
            if (Scene == null) return;
            Scene.Drawables.ForEach(
                drawable =>
                {
                    Render(drawable, canvas);
                }
                );
        }

        public void Render(IDrawableComponent drawable, ICanvas canvas)
        {
            canvas.StrokeSize = BaseGridSize;
            var drawPos = drawable.Position;
            var drawSize = drawable.Size;
            drawSize.x = drawSize.x * Zoom * BaseGridSize;
            drawSize.y = drawSize.y * Zoom * BaseGridSize;
            drawPos.x = drawPos.x * Zoom * BaseGridSize;
            drawPos.y = drawPos.y * Zoom * BaseGridSize;
            foreach (var instruction in drawable.DrawInstructions)
            {
                if (instruction is Line)
                {
                    Log.L("line");
                    var start = instruction.Coordinates[0];
                    var end = instruction.Coordinates[1];
                    SetStrokeColor(canvas, instruction.Colors[0]);
                    canvas.DrawLine(
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
                    SetStrokeColor(canvas, instruction.Colors[0]);
                    SetFillColor(canvas, instruction.Colors[1]);
                    canvas.DrawRectangle(
                        GetAbs(drawPos.x, drawSize.x, upperLeft.x),
                    GetAbs(drawPos.y, drawSize.y, upperLeft.y),
                    GetScale(drawSize.x, lowerRight.x),
                    GetScale(drawSize.y, lowerRight.y));
                }

                if (instruction is Text text)
                {
                    Log.L("text");
                    var textInstruction = text;
                    var centerPos = instruction.Coordinates[0];
                    var x = centerPos.x;
                    var y = centerPos.y;
                    canvas.SaveState();

                    SetStrokeColor(canvas, instruction.Colors[0]);

                    //	canvas.DrawString();
                }

                if (instruction is Circle)
                {
                    Log.L("circle");
                    var posCenter = instruction.Coordinates[0];
                    posCenter.x = GetAbs(drawPos.x, drawSize.x, posCenter.x);
                    posCenter.y = GetAbs(drawPos.y, drawSize.y, posCenter.y);
                    SetStrokeColor(canvas, instruction.Colors[0]);
                    canvas.DrawCircle(posCenter.x, posCenter.y, Zoom * BaseGridSize * 0.1f);
                }

                if (instruction is Path)
                {
                    SetStrokeColor(canvas, instruction.Colors[0]);
                    Path scenePath = (Path)instruction;

                    PathF path = new();
                    canvas.DrawPath(path);

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
                SetStrokeColor(canvas, pin.DrawInstructions[0].Colors[0]);
                canvas.DrawCircle(posCenter.x, posCenter.y, Zoom * BaseGridSize * 0.1f);
            }

            //	IImage image = ((PictureCanvas)canvas).Picture;
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