namespace ACDCs.Data.ACDCs.Components.Resistor;

public class ResistorTolerance : Dictionary<ResistorSeries, List<double>>
{
    public ResistorTolerance()
    {
        Add(ResistorSeries.E6, new List<double> { 20 });
        Add(ResistorSeries.E12, new List<double> { 10 });
        Add(ResistorSeries.E24, new List<double> { 5, 1 });
        Add(ResistorSeries.E48, new List<double> { 2 });
        Add(ResistorSeries.E96, new List<double> { 1 });
        Add(ResistorSeries.E192, new List<double> { 0.5, 0.25, 0.1 });
    }
}
