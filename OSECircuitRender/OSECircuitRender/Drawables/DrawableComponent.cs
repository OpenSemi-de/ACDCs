using System;
using OSECircuitRender.Definitions;
using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace OSECircuitRender.Drawables
{
    public class DrawableComponent : IDrawableComponent
    {
        public Coordinate Position { get; } = new(0, 0, 0);
        public Coordinate Size { get; } = new(1, 1, 0);
        public DrawablePinList DrawablePins { get; } = new();
        public DrawInstructionsList DrawInstructions { get; } = new();

        public DrawableComponent(Type type)
        {
            Type = type.Name;
        }

        public void SetRef(IWorksheetItem backRef)
        {
            BackRef = backRef;
            DrawablePins.ForEach(pin => pin.SetRef(backRef));
        }

        public void SetPosition(float x, float y)
        {
            Position.X = x;
            Position.Y = y;
        }

        public string Type { get; }

        [JsonIgnore]
        public IWorksheetItem BackRef { get; internal set; }

        public string RefName
        {
            get => BackRef == null ? "" : BackRef.RefName;

            set
            {
            }
        }

        public void SetSize(int width, int height)
        {
            Size.X = width;
            Size.Y = height;
        }
    }
}