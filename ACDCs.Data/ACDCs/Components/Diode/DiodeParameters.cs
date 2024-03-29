﻿using System.Diagnostics.CodeAnalysis;
using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Diode;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class DiodeParameters : IComponentParameters
{
    public double ActivationEnergy { get; set; }
    public double BreakdownCurrent { get; set; }
    public double BreakdownVoltage { get; set; }
    public double DepletionCapCoefficient { get; set; }
    public double EmissionCoefficient { get; set; }
    public double FlickerNoiseCoefficient { get; set; }
    public double FlickerNoiseExponent { get; set; }
    public double GradingCoefficient { get; set; }
    public double JunctionCap { get; set; }
    public double JunctionPotential { get; set; }
    public double NominalTemperature { get; set; }
    public double NominalTemperatureCelsius { get; set; }
    public double Resistance { get; set; }
    public double SaturationCurrent { get; set; }
    public double SaturationCurrentExp { get; set; }
    public double TransitTime { get; set; }
}
