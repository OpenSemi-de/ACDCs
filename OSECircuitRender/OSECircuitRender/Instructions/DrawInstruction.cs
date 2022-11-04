﻿using System;
using System.Collections.Generic;
using OSECircuitRender.Definitions;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Instructions
{
    public class DrawInstruction : IDrawInstruction
    {
        public DrawInstruction(Type type)
        {
            Type = type.Name;
        }

        //public List<Coordinate> Coordinates { get; set; } = new();

        //public List<Color> Colors { get; } = new();
        public Coordinate Position { get; set; }

        public Color StrokeColor { get; set; }
        public string Type { get; }
    }
}