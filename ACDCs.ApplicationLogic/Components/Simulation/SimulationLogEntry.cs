namespace ACDCs.API.Core.Components.Simulation;

public class SimulationLogEntry
{
    public DateTime Date { get; set; }

    public string Text { get; set; }

    public SimulationLogEntry(DateTime date, string text)
    {
        Date = date;
        Text = text;
    }
}
