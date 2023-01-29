using ACDCs.Data.ACDCs.Interfaces;

namespace ACDCs.Data.ACDCs.Components.Mosfet;

public class MosFetParametersLevel3 : IComponentParameters
{
    public bool BadMos { get; set; }
    public float BulkCapFactor { get; set; }
    public float BulkJunctionBotGradingCoefficient { get; set; }
    public float BulkJunctionPotential { get; set; }
    public float BulkJunctionSideGradingCoefficient { get; set; }
    public float CapBd { get; set; }
    public float CapBs { get; set; }
    public float Delta { get; set; }
    public float DelVt0 { get; set; }
    public float DrainResistance { get; set; }
    public float Eta { get; set; }
    public float FastSurfaceStateDensity { get; set; }
    public float FlickerNoiseCoefficient { get; set; }
    public float FlickerNoiseExponent { get; set; }
    public float ForwardCapDepletionCoefficient { get; set; }
    public float Gamma { get; set; }
    public float GateBulkOverlapCapFactor { get; set; }
    public float GateDrainOverlapCapFactor { get; set; }
    public float GateSourceOverlapCapFactor { get; set; }
    public float GateType { get; set; }
    public float JunctionDepth { get; set; }
    public float JunctionSatCur { get; set; }
    public float JunctionSatCurDensity { get; set; }
    public float Kappa { get; set; }
    public float LateralDiffusion { get; set; }
    public float Length { get; set; }
    public float LengthAdjust { get; set; }
    public float MaxDriftVelocity { get; set; }
    public float MosfetType { get; set; }
    public float NominalTemperature { get; set; }
    public float NominalTemperatureCelsius { get; set; }
    public float OxideThickness { get; set; }
    public float Phi { get; set; }
    public float SheetResistance { get; set; }
    public float SidewallCapFactor { get; set; }
    public float SourceResistance { get; set; }
    public float SubstrateDoping { get; set; }
    public float SurfaceMobility { get; set; }
    public float SurfaceStateDensity { get; set; }
    public float Theta { get; set; }
    public float Transconductance { get; set; }
    public string TypeName { get; set; }
    public int Version { get; set; }
    public float Vt0 { get; set; }
    public float Width { get; set; }
    public float WidthAdjust { get; set; }
    public float WidthNarrow { get; set; }
}
