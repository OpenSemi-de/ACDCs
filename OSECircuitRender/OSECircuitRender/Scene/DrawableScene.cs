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
            GetScaleAndZoom(drawable, out Coordinate drawPos, out Coordinate drawSize);

            foreach (var instruction in drawable.DrawInstructions)
            {
                if (instruction is LineInstruction line)
                {
                    Log.L("line");

                    SetStrokeColor(Canvas, instruction.Colors[0]);
                    Canvas.DrawLine(
                        GetAbs(drawPos.X, drawSize.X, line.X1),
                        GetAbs(drawPos.Y, drawSize.Y, line.Y1),
                        GetAbs(drawPos.X, drawSize.X, line.X2),
                        GetAbs(drawPos.Y, drawSize.Y, line.Y2));
                }

                if (instruction is BoxInstruction)
                {
                    Log.L("box");
                    var upperLeft = new Coordinate(instruction.Coordinates[0]);
                    var lowerRight = new Coordinate(instruction.Coordinates[1]);
                    SetStrokeColor(Canvas, instruction.Colors[0]);
                    SetFillColor(Canvas, instruction.Colors[1]);
                    Canvas.DrawRectangle(
                        GetAbs(drawPos.X, drawSize.X, upperLeft.X),
                        GetAbs(drawPos.Y, drawSize.Y, upperLeft.Y),
                        GetScale(drawSize.X, lowerRight.X),
                        GetScale(drawSize.Y, lowerRight.Y));
                }

                if (instruction is TextInstruction text)
                {
                    Log.L("text");
                    var centerPos = new Coordinate(instruction.Coordinates[0]);
                    var x = GetAbs(drawPos.X, drawSize.X, centerPos.X);
                    var y = GetAbs(drawPos.Y, drawSize.Y, centerPos.Y);
                    Canvas.SaveState();
                    Canvas.FontSize = text.Size;
                    Canvas.Translate(x, y);
                    Canvas.Rotate(text.Orientation);
                    SetStrokeColor(Canvas, instruction.Colors[0]);
                    Canvas.DrawString(text.Text, 0, 0, HorizontalAlignment.Center);
                    Canvas.Translate(0, 0);
                    Canvas.ResetState();
                }

                if (instruction is CircleInstruction)
                {
                    Log.L("circle");
                    var posCenter = new Coordinate(instruction.Coordinates[0]);
                    posCenter.X = GetAbs(drawPos.X, drawSize.X, posCenter.X);
                    posCenter.Y = GetAbs(drawPos.Y, drawSize.Y, posCenter.Y);
                    SetStrokeColor(Canvas, instruction.Colors[0]);
                    Canvas.DrawCircle(posCenter.X, posCenter.Y, Zoom * BaseGridSize * 0.1f);
                }

                if (instruction is PathInstruction path)
                {
                    SetStrokeColor(Canvas, instruction.Colors[0]);
                    PathInstruction scenePath = path;

                    PathF pathF = new(drawPos.X, drawPos.Y);

                    foreach (var part in path.GetParts())
                    {
                        switch (part.Type)
                        {
                            case PathPartType.A:
                                break;

                            case PathPartType.C:
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

                    Canvas.DrawPath(pathF);
                }
            }

            foreach (var pin in drawable.DrawablePins)
            {
                Log.L("pin");
                var posCenter = new Coordinate(pin.Position);
                posCenter.X = GetAbs(drawPos.X, drawSize.X, posCenter.X);
                posCenter.Y = GetAbs(drawPos.Y, drawSize.Y, posCenter.Y);
                SetStrokeColor(Canvas, pin.DrawInstructions[0].Colors[0]);
                Canvas.DrawCircle(posCenter.X, posCenter.Y, Zoom * BaseGridSize * 0.1f);
            }
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
            canvas.StrokeColor = new Microsoft.Maui.Graphics.Color(fillColor.R, fillColor.G, fillColor.B);
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