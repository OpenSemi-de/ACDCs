namespace ACDCs.API.Core.Components.Sensors;

public class SensorsConfigurationView : Grid
{
    private readonly ListView _usedSensorsListView;

    public SensorsConfigurationView()
    {
        RowDefinition[] rows =
        {
            new(40) ,
            new(GridLength.Star),
            new(40)
        };

        ColumnDefinition[] columns =
        {
            new(GridLength.Star),
            new(60),
            new(GridLength.Star)
        };

        this.RowDefinitions(rows)
            .ColumnDefinitions(columns)
            .ColumnSpacing(0)
            .RowSpacing(0)
            .Padding(0)
            .Margin(2);

        _usedSensorsListView = new ListView()
            .Margin(2)
            .ItemTemplate(new UsedSensorsTemplate());
    }
}
