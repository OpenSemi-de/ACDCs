namespace ACDCs.API.Core.Components.Sensors;

public class SensorsDataTemplate : DataTemplate
{
    public string Description { get; set; }
    public string Location { get; set; }
    public string Name { get; set; }
    public SensorSpeed SensorSpeed { get; set; }
    public string Type { get; set; }
    public string TypeDescription { get; set; }

    public SensorsDataTemplate()
    {
        Add(GetLayout);
    }

    private object GetLayout()
    {
        RowDefinition[] rows =
            {
            new (16),
            new(30)
        };

        return new Grid()
        {
            new Label()
                .Bind(Label.TextProperty, "Name")
                .FontSize(11),
            new Label()
                .Row(1)
                .Bind(Label.TextProperty, "Location")
                .FontSize(10)
        }
              .RowDefinitions(rows);
    }
}
