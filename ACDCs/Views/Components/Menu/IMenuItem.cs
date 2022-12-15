namespace ACDCs.Views.Components.Menu;

public interface IMenuItem
{
    double ItemHeight { get; set; }
    string MenuCommand { get; set; }
    string Text { get; set; }
}