using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSEData.OSE.Components
{
    public class Bjt : ElectricalComponent, IElectronicComponent
    {
        public IComponentRuntimeParameters ParametersRuntime { get; set; }
        public IComponentParameters ParametersModel { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
    }

    public class BjtRuntimeParameters : IComponentRuntimeParameters
    {
        public float TemperatureCelsius { get; set; }
        public bool Off { get; set; }
        public float InitialVoltageCe { get; set; }
        public float InitialVoltageBe { get; set; }
        public float Area { get; set; }
        public float Temperature { get; set; }
        public float ParallelMultiplier { get; set; }
    }

    public class BjtModelParameters : IComponentParameters
    {
        public string TypeName { get; set; }
        public float BipolarType { get; set; }
        public float NominalTemperatureCelsius { get; set; }
        public float TempExpIs { get; set; }
        public float PotentialBe { get; set; }
        public float SatCur { get; set; }
        public float LeakBeEmissionCoefficient { get; set; }
        public float JunctionExpBc { get; set; }
        public float PotentialSubstrate { get; set; }
        public float EmissionCoefficientForward { get; set; }
        public float EarlyVoltageForward { get; set; }
        public float LeakBcEmissionCoefficient { get; set; }
        public float C2 { get; set; }
        public float C4 { get; set; }
        public float EmissionCoefficientReverse { get; set; }
        public float DepletionCapCoefficient { get; set; }
        public float LeakBcCurrent { get; set; }
        public float RollOffReverse { get; set; }
        public float DepletionCapBc { get; set; }
        public float EarlyVoltageReverse { get; set; }
        public float BetaR { get; set; }
        public float TransitTimeForwardVoltageBc { get; set; }
        public float TransitTimeForward { get; set; }
        public float RollOffForward { get; set; }
        public float EnergyGap { get; set; }
        public float BaseFractionBcCap { get; set; }
        public float JunctionExpBe { get; set; }
        public float ExponentialSubstrate { get; set; }
        public float FlickerNoiseExponent { get; set; }
        public float TransitTimeReverse { get; set; }
        public float FlickerNoiseCoefficient { get; set; }
        public float BetaExponent { get; set; }
        public float BaseCurrentHalfResist { get; set; }
        public float EmitterResistance { get; set; }
        public float TransitTimeBiasCoefficientForward { get; set; }
        public float TransitTimeHighCurrentForward { get; set; }
        public float MinimumBaseResistance { get; set; }
        public float BaseResist { get; set; }
        public float NominalTemperature { get; set; }
        public float PotentialBc { get; set; }
        public float BetaF { get; set; }
        public float LeakBeCurrent { get; set; }
        public float CapCs { get; set; }
        public float DepletionCapBe { get; set; }
        public float ExcessPhase { get; set; }
        public float CollectorResistance { get; set; }
    }
}