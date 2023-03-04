namespace ACDCs.API.Core.Components.Sensors;

public class AvailableSensorsViewCell : ViewCell
{
    public AvailableSensorsViewCell()
    {
        ColumnDefinition[] columns =
        {
            new (80),
            new(GridLength.Star)
        };

        Add(
            new Grid()
                {
                    new Label().Bind(Label.TextProperty, "Name"),
                    new Label().Column(1).Bind(Label.TextProperty, "Location")
                }
                .ColumnDefinitions(columns)
        );
    }
}
