using System.Collections.Generic;
using System;

namespace OSECircuitRender
{
    public class DrawInstruction : IDrawInstruction
    {
        public DrawInstruction(Type type)
        {
            _type = type.Name;
        }

        public List<DrawCoordinate> Coordinates { get; set; } = new List<DrawCoordinate>();

        public List<Color> Colors
        {
            get;
        } = new List<Color>();

        public string _type { get; }
    }
}