using System;
using OSECircuitRender.Items;

namespace OSECircuitRender.Interfaces
{
    public interface IWorksheetItem
    {
        public Guid ItemGuid { get; set; }
        public string Name { get; set; }
        IDrawableComponent DrawableComponent { get; set; }

        float X { get; set; }
        float Y { get; set; }

        float Rotation { get; set; }

        string RefName { get; set; }

        public DrawablePinList Pins { get; set; }
    }
}