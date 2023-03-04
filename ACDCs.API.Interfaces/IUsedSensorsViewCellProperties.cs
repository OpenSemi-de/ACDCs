namespace ACDCs.API.Interfaces;

using Sharp.UI;

[BindableProperties]
public interface IUsedSensorsViewCellProperties
{
    public string Description { get; set; }
    public string Location { get; set; }
    public string Name { get; set; }
    public SensorSpeed SensorSpeed { get; set; }
    public string Type { get; set; }
    public string TypeDescription { get; set; }
}
