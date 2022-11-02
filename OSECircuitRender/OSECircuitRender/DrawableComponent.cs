using System;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace OSECircuitRender
{
    public class DrawableComponent : IDrawableComponent
    {
        public DrawCoordinate Position { get; } = new DrawCoordinate(0, 0, 0);
        public DrawCoordinate Size { get; } = new DrawCoordinate(1, 1, 0);
        public DrawablePins DrawablePins { get; } = new DrawablePins();
        public DrawInstructions DrawInstructions { get; } = new DrawInstructions();

        public DrawableComponent(Type type)
        {
            _type = type.Name;
        }

        public void SetRef(IWorksheetItem backRef)
        {
            _backRef = backRef;
            DrawablePins.ForEach(pin => pin.SetRef(backRef));
        }

        public void SetPosition(float x, float y)
        {
            Position.x = x;
            Position.y = y;
        }

        public string _type { get; }

        [JsonIgnore]
        public IWorksheetItem _backRef { get; internal set; }

        public string RefName
        {
            get
            {
                return _backRef.RefName;
            }
        }

        public void SetSize(int width, int height)
        {
            Size.x = width;
            Size.y = height;
        }
    }
}