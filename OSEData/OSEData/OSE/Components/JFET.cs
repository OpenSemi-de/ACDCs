using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSEData.OSE.Components
{
    public class JFET : ElectricalComponent, IElectronicComponent
    {
        public IComponentRuntimeParameters ParametersRuntime => new JFETParametersRuntimeParameters();

        public IComponentParameters ParametersModel => new JFETParameters();
    }

    public class JFETParametersRuntimeParameters : IComponentRuntimeParameters
    {
        public float TemperatureCelsius { get; set; }
        public bool Off { get; set; }
        public float Area { get; set; }
        public float Temperature { get; set; }
        public float InitialVgs { get; set; }
        public float InitialVds { get; set; }
        public float ParallelMultiplier { get; set; }
    }

    public class JFETParameters : IComponentParameters
    {
        public float NominalTemperatureCelsius { get; set; }
        public float JFETType { get; set; }
        public float DrainConductance { get; set; }
        public float SourceConductance { get; set; }
        public string TypeName { get; set; }
        public float Beta { get; set; }
        public float NominalTemperature { get; set; }
        public float Threshold { get; set; }
        public float LModulation { get; set; }
        public float CapGd { get; set; }
        public float SourceResistance { get; set; }
        public float CapGs { get; set; }
        public float FnCoefficient { get; set; }
        public float DepletionCapCoefficient { get; set; }
        public float FnExponent { get; set; }
        public float B { get; set; }
        public float DrainResistance { get; set; }
        public float GateSaturationCurrent { get; set; }
        public float GatePotential { get; set; }
    }
}