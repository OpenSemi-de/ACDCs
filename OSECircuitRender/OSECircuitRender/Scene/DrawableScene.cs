using Microsoft.Maui.Graphics;
using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;

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
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.WhiteSmoke;
            canvas.FillRectangle(0, 0, 10000, 10000);
            if (Scene == null) return;
            Scene.Drawables.ForEach(
                component => Render(canvas, component)
            );
        }

        public void Render(ICanvas canvas, IDrawableComponent drawable)
        {
            canvas.StrokeSize = BaseGridSize / 2;
            GetScaleAndZoom(drawable, out Coordinate drawPos, out Coordinate drawSize);

            foreach (var instruction in drawable.DrawInstructions)
            {
                if (instruction is LineInstruction line)
                {
                    Log.L("line");
                    SetStrokeColor(canvas, instruction.StrokeColor);
                    DrawLine(canvas, drawPos, drawSize, line);
                }

                if (instruction is BoxInstruction box)
                {
                    Log.L("box");
                    var upperLeft = new Coordinate(box.Position);
                    var lowerRight = new Coordinate(box.Size);
                    SetStrokeColor(canvas, box.StrokeColor);
                    SetFillColor(canvas, box.FillColor);
                    DrawRectangle(canvas, drawPos, drawSize, upperLeft, lowerRight);
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
                    DrawPath(canvas, drawPos, path);
                }
            }

            foreach (var pin in drawable.DrawablePins)
            {
                Log.L("pin");
                var posCenter = new Coordinate(pin.Position);
                posCenter.X = GetAbs(drawPos.X, drawSize.X, posCenter.X);
                posCenter.Y = GetAbs(drawPos.Y, drawSize.Y, posCenter.Y);
                SetStrokeColor(canvas, pin.DrawInstructions[0].StrokeColor);
                canvas.DrawCircle(posCenter.X, posCenter.Y, Zoom * BaseGridSize * 0.1f);
            }
        }

        private static void DrawPath(ICanvas canvas, Coordinate drawPos, PathInstruction path)
        {
            PathF pathF = new(drawPos.X, drawPos.Y);

            foreach (var part in path.GetParts())
            {
                switch (part.Type)
                {
                    case PathPartType.A:
                        break;

                    case PathPartType.C:
                        {
                            pathF.CurveTo(
                                drawPos.X + part.Coordinates[0].X,
                                drawPos.Y + part.Coordinates[0].Y,
                                drawPos.X + part.Coordinates[1].X,
                                drawPos.Y + part.Coordinates[1].Y,
                                drawPos.X + part.Coordinates[2].X,
                                drawPos.Y + part.Coordinates[2].Y
                            );
                        }
                        break;

                    case PathPartType.M:
                        {
                            pathF.MoveTo(drawPos.X + part.Coordinates[0].X, drawPos.Y + part.Coordinates[0].Y);
                        }
                        break;

                    case PathPartType.L:
                        {
                            pathF.LineTo(drawPos.X + part.Coordinates[0].X, drawPos.Y + part.Coordinates[0].Y);
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
            var x = GetAbs(drawPos.X, drawSize.X, centerPos.X);
            var y = GetAbs(drawPos.Y, drawSize.Y, centerPos.Y);
            canvas.DrawCircle(x, y, Zoom * BaseGridSize * 0.1f);
        }

        private static void DrawText(ICanvas canvas, Coordinate drawPos, Coordinate drawSize, TextInstruction text, Coordinate centerPos, IDrawInstruction instruction)
        {
            var x = GetAbs(drawPos.X, drawSize.X, centerPos.X);
            var y = GetAbs(drawPos.Y, drawSize.Y, centerPos.Y);
            canvas.SaveState();
            canvas.FontSize = text.Size;
            canvas.Translate(x, y);
            canvas.Rotate(text.Orientation);
            canvas.DrawString(text.Text, 0, 0, HorizontalAlignment.Center);
            canvas.Translate(0, 0);
            canvas.ResetState();
        }

        private static void DrawRectangle(ICanvas canvas, Coordinate drawPos, Coordinate drawSize, Coordinate upperLeft,
            Coordinate lowerRight)
        {
            canvas.DrawRectangle(
                GetAbs(drawPos.X, drawSize.X, upperLeft.X),
                GetAbs(drawPos.Y, drawSize.Y, upperLeft.Y),
                GetScale(drawSize.X, lowerRight.X),
                GetScale(drawSize.Y, lowerRight.Y));
        }

        private static void DrawLine(ICanvas canvas, Coordinate drawPos, Coordinate drawSize, LineInstruction line)
        {
            canvas.DrawLine(
                GetAbs(drawPos.X, drawSize.X, line.Position.X),
                GetAbs(drawPos.Y, drawSize.Y, line.Position.Y),
                GetAbs(drawPos.X, drawSize.X, line.End.X),
                GetAbs(drawPos.Y, drawSize.Y, line.End.Y));
        }

        private static void GetScaleAndZoom(IDrawableComponent drawable, out Coordinate drawPos, out Coordinate drawSize)
        {
            drawPos = new Coordinate(drawable.Position);
            drawSize = new Coordinate(drawable.Size);
            drawSize.X = drawSize.X * Zoom * BaseGridSize;
            drawSize.Y = drawSize.Y * Zoom * BaseGridSize;
            drawPos.X = drawPos.X * Zoom * BaseGridSize;
            drawPos.Y = drawPos.Y * Zoom * BaseGridSize;
        }

        public static void SetFillColor(ICanvas canvas, Definitions.Color fillColor)
        {
            canvas.FillColor = new Microsoft.Maui.Graphics.Color(fillColor.R, fillColor.G, fillColor.B);
        }

        public static void SetStrokeColor(ICanvas canvas, Definitions.Color penColor)
        {
            canvas.StrokeColor = new Microsoft.Maui.Graphics.Color(penColor.R, penColor.G, penColor.B);
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