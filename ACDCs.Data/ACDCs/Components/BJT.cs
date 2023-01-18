namespace ACDCs.Data.ACDCs.Components;

public class Bjt : BjtModelParameters, IElectronicComponent
{
    public string Model { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IComponentRuntimeParameters ParametersRuntime => new BjtRuntimeParameters();
    public string Type { get; set; } = string.Empty;

    public string Value
    {
        get => Name;
        set => Name = value;
    }
}

public class BjtModelParameters : IComponentParameters
{
    public double BaseCurrentHalfResist { get; set; }
    public double BaseFractionBcCap { get; set; }
    public double BaseResist { get; set; }
    public double BetaExponent { get; set; }
    public double BetaF { get; set; }
    public double BetaR { get; set; }
    public double BipolarType { get; set; }
    public double C2 { get; set; }
    public double C4 { get; set; }
    public double CapCs { get; set; }
    public double CollectorResistance { get; set; }
    public double DepletionCapBc { get; set; }
    public double DepletionCapBe { get; set; }
    public double DepletionCapCoefficient { get; set; }
    public double EarlyVoltageForward { get; set; }
    public double EarlyVoltageReverse { get; set; }
    public double EmissionCoefficientForward { get; set; }
    public double EmissionCoefficientReverse { get; set; }
    public double EmitterResistance { get; set; }
    public double EnergyGap { get; set; }
    public double ExcessPhase { get; set; }
    public double ExponentialSubstrate { get; set; }
    public double FlickerNoiseCoefficient { get; set; }
    public double FlickerNoiseExponent { get; set; }
    public double JunctionExpBc { get; set; }
    public double JunctionExpBe { get; set; }
    public double LeakBcCurrent { get; set; }
    public double LeakBcEmissionCoefficient { get; set; }
    public double LeakBeCurrent { get; set; }
    public double LeakBeEmissionCoefficient { get; set; }
    public double MinimumBaseResistance { get; set; }
    public double NominalTemperature { get; set; }
    public double NominalTemperatureCelsius { get; set; }
    public double PotentialBc { get; set; }
    public double PotentialBe { get; set; }
    public double PotentialSubstrate { get; set; }
    public double RollOffForward { get; set; }
    public double RollOffReverse { get; set; }
    public double SatCur { get; set; }
    public double TempExpIs { get; set; }
    public double TransitTimeBiasCoefficientForward { get; set; }
    public double TransitTimeForward { get; set; }
    public double TransitTimeForwardVoltageBc { get; set; }
    public double TransitTimeHighCurrentForward { get; set; }
    public double TransitTimeReverse { get; set; }
    public string TypeName { get; set; } = string.Empty;
}

public class BjtRuntimeParameters : IComponentRuntimeParameters
{
    public double Area { get; set; }
    public double InitialVoltageBe { get; set; }
    public double InitialVoltageCe { get; set; }
    public bool Off { get; set; }
    public double ParallelMultiplier { get; set; }
    public double Temperature { get; set; }
    public double TemperatureCelsius { get; set; }
}
