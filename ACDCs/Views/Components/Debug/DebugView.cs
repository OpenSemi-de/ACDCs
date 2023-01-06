using ACDCs.Views.Components.Window;
using Microsoft.Maui.Layouts;
using UraniumUI.Material.Controls;

namespace ACDCs.Views.Components.Debug;

using Sharp.UI;

public class DebugWindow : WindowView
{
    private readonly Grid _grid;
    private readonly Label _label;
    private readonly TextField _textField;

    public DebugWindow(SharpAbsoluteLayout layout) : base(layout, "Debug")
    {
        _grid = new Grid()
            .ColumnDefinitions(new ColumnDefinitionCollection(
                new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star)
            ))
            .RowDefinitions(new RowDefinitionCollection(
                new Microsoft.Maui.Controls.RowDefinition(GridLength.Star),
                new Microsoft.Maui.Controls.RowDefinition(new GridLength(40))
            ))
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);
        AbsoluteLayout.SetLayoutBounds(this, new Rect(1, 1, 300, 400));

        _label = new Label("ACDCs Debug console" + Environment.NewLine)
            .MaxLines(int.MaxValue);

        _textField = new TextField();
        Grid.SetRow(_textField, 1);
        _grid.Add(_label);
        _grid.Add(_textField);

        WindowContent = _grid;
    }
}
