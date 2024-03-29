﻿namespace ACDCs.Sensors.API.Samples;

using Interfaces;

public class BarometerSample : ISample<double>, ISample
{
    public double Sample { get; set; }

    public DateTime Time { get; set; }
}
