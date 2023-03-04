// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
namespace ACDCs.API.Core.Components.Sensors;

public class SensorItem
{
    public string Description { get; set; }

    public string Location { get; set; }

    public string Name { get; set; }

    public SensorSpeed SensorSpeed { get; set; }

    public string Type { get; set; }

    public string TypeDescription { get; set; }

    public SensorItem(string description, string location, string name, SensorSpeed sensorSpeed, string type, string typeDescription)
    {
        Description = description;
        Location = location;
        Name = name;
        SensorSpeed = sensorSpeed;
        Type = type;
        TypeDescription = typeDescription;
    }
}
