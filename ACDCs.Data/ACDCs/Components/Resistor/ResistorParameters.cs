﻿using System.Diagnostics.CodeAnalysis;
using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Resistor;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class ResistorParameters : AdvancedResistorParameters, IComponentParameters
{
    public double DefaultWidth { get; set; }
    public double ExponentialCoefficient { get; set; }
    public double Narrow { get; set; }
    public double NominalTemperature { get; set; }
    public double NominalTemperatureCelsius { get; set; }
    public double SheetResistance { get; set; }
    public double TemperatureCoefficient1 { get; set; }
    public double TemperatureCoefficient2 { get; set; }
}
