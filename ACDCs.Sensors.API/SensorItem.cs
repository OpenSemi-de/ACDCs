// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
namespace ACDCs.Sensors.API;

public class SensorItem
{
    public string Description { get; set; }

    public string Location { get; set; }

    public string Name { get; set; }

    public SensorSpeed SensorSpeed { get; set; }

    public string SensorType { get; set; }

    public string TypeDescription { get; set; }

    public SensorItem(string description, string location, string name, SensorSpeed sensorSpeed, string sensorType, string typeDescription)
    {
        Description = description;
        Location = location;
        Name = name;
        SensorSpeed = sensorSpeed;
        SensorType = sensorType;
        TypeDescription = typeDescription;
    }
}
