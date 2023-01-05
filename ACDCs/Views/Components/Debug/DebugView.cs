using ACDCs.Views.Components.Window;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Components.Debug;

using Sharp.UI;

public class DebugWindow : WindowView
{
    public DebugWindow(SharpAbsoluteLayout layout) : base(layout, "Debug")
    {
        Grid grid = new Grid()
            .ColumnDefinitions(new ColumnDefinitionCollection(
                new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star)
            ))
            .RowDefinitions(new RowDefinitionCollection(
                new Microsoft.Maui.Controls.RowDefinition(GridLength.Star)
            ))
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(this, new Rect(1, 1, 300, 400));

        WindowContent = grid;
    }
}
