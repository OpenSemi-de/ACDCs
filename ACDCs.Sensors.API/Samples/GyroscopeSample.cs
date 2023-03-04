﻿namespace ACDCs.Sensors.API.Samples;

using System.Numerics;
using Interfaces;

public class GyroscopeSample : ISample<Vector3>
{
    public Vector3 Sample { get; set; }
    public DateTime Time { get; set; }
}